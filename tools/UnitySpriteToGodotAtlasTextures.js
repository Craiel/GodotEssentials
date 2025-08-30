'use strict';

const fs = require('fs');
const path = require('path');
const sharp = require('sharp');
const EssentialsRoot = path.dirname(__dirname) + path.sep;
const PrefabTemplateFile = path.join(__dirname, 'AtlasTextureTemplate.tre_');
const ProjectBasePath = "Project" + path.sep;
let PrefabTemplate = fs.readFileSync(PrefabTemplateFile).toString();

let MetaFolder = process.argv[2];
let AssetFolder = process.argv[3];
let TargetFolder = process.argv[4];
if(MetaFolder === undefined || AssetFolder === undefined || TargetFolder === undefined) {
    return;
}

class UnitySpriteToTextureAtlas {
    constructor() {
        this.filesToProcess = [];
        this.extensionsToIndex = ['.meta'];
        this.typeToIndex = {};
    }
    
    execute() {
        this.findFiles(MetaFolder);
        this.processResults();
    }

    findFiles(dir) {
        let contents = fs.readdirSync(dir);
        for (let f = 0; f < contents.length; f++) {
            let fullPath = path.join(dir, contents[f]);
            let pathLStat = fs.lstatSync(fullPath);
            if(pathLStat.isDirectory() === true) {
                this.findFiles(fullPath);
            } else {
                for(let i = 0; i < this.extensionsToIndex.length; i++){
                    if(fullPath.endsWith(this.extensionsToIndex[i])){
                        this.filesToProcess.push(fullPath);
                        break;
                    }
                }
            }
        }
    }

    getUID(asset) {
        let importSettingsFile = asset + ".import";
        if(!fs.existsSync(importSettingsFile)) {
            console.warn("Asset is missing Import Settings: " + asset);
            return undefined;
        }

        let importSettings = fs.readFileSync(importSettingsFile).toString().split("\n");
        for(let i = 0; i < importSettings.length; i++){
            let line = importSettings[i].trim().replace("\r", "");
            if(line.startsWith("uid=")) {
                let lineVal = line.split("=");
                return lineVal[1].replace(/^"|"$/g, '');
            }
        }
    }

    getProjectBasePath(assetPath) {
        let basePathIndex = assetPath.indexOf(ProjectBasePath) + ProjectBasePath.length;
        let result = assetPath.substring(basePathIndex, assetPath.length);
        result = 'res://' + result.replaceAll(path.sep, '/');
        return result;
    }

    getMetaSpriteInfos(metaFile) {
        let data = fs.readFileSync(metaFile).toString().split("\n");

        let inSpriteSection = false;
        let results = [];
        let entryInfo = {};
        for(let i = 0; i < data.length; i++){
            let line = data[i].trim();
            //console.log(line);
            if(line.startsWith("spriteSheet:") > 0) {
                inSpriteSection = true;
                continue;
            }

            if(line.startsWith("spritePackingTag:") > 0) {
                break;
            }

            if(inSpriteSection === false) {
                continue;
            }

            if(line.startsWith("- serializedVersion:") > 0) {
                if(entryInfo.name !== undefined) {
                    results.push(entryInfo);
                }

                entryInfo = {};
                continue;
            }

            if(line.startsWith("name: ")) {
                entryInfo.name = line.substring(6).trim();
                continue;
            }

            if(line.startsWith("x: ")) {
                entryInfo.x = line.substring(3).trim();
                continue;
            }

            if(line.startsWith("y: ")) {
                entryInfo.y = line.substring(3).trim();
                continue;
            }

            if(line.startsWith("width: ")) {
                entryInfo.w = line.substring(6).trim();
                continue;
            }

            if(line.startsWith("height: ")) {
                entryInfo.h = line.substring(7).trim();
                continue;
            }
        }

        return results;
    }

    async processResults() {
        fs.rmSync(TargetFolder, { recursive: true, force: true });
        fs.mkdirSync(TargetFolder);

        for(let i = 0; i < this.filesToProcess.length; i++) {
            let metaFile = this.filesToProcess[i];
            let assetFile = path.join(AssetFolder, path.basename(metaFile).replace(".meta", ""));

            if(!fs.existsSync(assetFile)){
                console.warn("Missing Asset for Meta: " + assetFile);
                continue;
            }

            const image = await sharp(assetFile);
            const metadata = await image.metadata();
            console.log(metadata.width, metadata.height);

            let assetProjectPath = this.getProjectBasePath(assetFile);

            let targetFolder = path.join(TargetFolder, path.basename(assetFile.replace(".png", "")));
            fs.mkdirSync(targetFolder);

            let uid = this.getUID(assetFile);

            let sprites = this.getMetaSpriteInfos(metaFile);
            console.log(sprites);

            for(let is = 0; is < sprites.length; is++){
                let sprite = sprites[is];
                let prefab = PrefabTemplate
                    .replace('#UID#', uid)
                    .replace('#PATH#', assetProjectPath)
                    .replace('#X#', sprite.x)
                    .replace('#Y#', metadata.height - sprite.y - sprite.h)
                    .replace('#W#', sprite.w)
                    .replace('#H#', sprite.h);

                let prefabFile = path.join(targetFolder, sprite.name + ".tres");
                fs.writeFileSync(prefabFile, prefab);
            }
        }
    }
}

let gen = new UnitySpriteToTextureAtlas();
gen.execute();