'use strict';

const fs = require('fs');
const path = require('path');
const EssentialsRoot = path.dirname(__dirname) +  "\\";
const PrefabTemplateFile = __dirname + '\\LinkTemplate.tre_';
let PrefabTemplate = fs.readFileSync(PrefabTemplateFile).toString();

let SourceFolder = process.argv[2];
if(SourceFolder === undefined) {
    SourceFolder = "..\\Project\\source\\Game\\";
}

class PrefabGenerator {
    constructor() {
        this.results = {};
        this.filesToIndex = [];
        this.extensionsToIndex = ['.cs'];
        this.targetFolder = "..\\Project\\prefabs\\database\\";
        this.sourceFolder = SourceFolder + "Database\\";
        this.dataTypeFile = SourceFolder + 'Enums\\GameDataType.cs';
        this.typeToIndex = {};
    }

    indexFile(filePath) {
        let path = filePath.replace(this.sourceFolder, "");
        let segments = path.split('\\');
        let id = "";
        for(let i = 0; i < segments.length; i++) {
            if(i === 0 && segments.length > 1) {
                id = segments[i];
                continue;
            }

            if(i == segments.length - 1) {
                id = id + "_" + segments[i];
                continue;
            }

            id = id + "_" + segments[i].slice(0, 2);
        }

        for(let i = 0; i < this.extensionsToIndex.length; i++) {
            id = id.replace(this.extensionsToIndex[i], "");
        }

        id = id.toUpperCase();

        if(this.results[id] !== undefined) {
            throw "Duplicate ID: " + id;
        }

        this.results[id] = filePath;
    }
    
    execute() {
        this.rebuildTypeIndexMap();
        this.findFiles(this.sourceFolder);
        for(let i = 0; i < this.filesToIndex.length; i++) {
            this.indexFile(this.filesToIndex[i]);
        }

        this.writeResults();
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
                        this.filesToIndex.push(fullPath);
                        break;
                    }
                }
            }
        }
    }

    rebuildTypeIndexMap() {
        const valueRegex = /\s([a-z]+)\s+=\s+([0-9]+)/gi;
        let typeFileContents = fs.readFileSync(this.dataTypeFile).toString();

        let match = null;
        do {
            match = valueRegex.exec(typeFileContents);
            if(match !== null) {
                this.typeToIndex[match[1]] = parseInt(match[2]);
            }
        } while(match != null);
    }

    writeResults() {
        fs.rmSync(this.targetFolder, { recursive: true, force: true });
        fs.mkdirSync(this.targetFolder);

        const idMatchRegex = /\snew\s*\((.+?)\)/;
        for(let id in this.results) {
            let file = this.results[id];
            let fileContents = fs.readFileSync(file).toString().split("\n");
            let match = null;
            for(let i = 0; i < fileContents.length; i++) {
                if(fileContents[i].indexOf('GameDataType.') < 0) {
                    continue;
                }
                
                let line = fileContents[i].replaceAll("\t", " ").replaceAll("\r", "");
                match = idMatchRegex.exec(line);
                if(match !== null) {
                    break;
                }
                
                console.log("NO_MATCH: " + line);
            }
            
            if(match === null) {
                continue;
            }
            
            console.log(match[1]);
            let args = match[1].split(',');
            for(let i = 0; i < args.length; i++) {
                args[i] = args[i].trim().replaceAll('"', '');
            }

            let dataType = args[1].replace('GameDataType.', '');
            if(dataType === 'Unset') {
                continue;
            }

            let category = id.split('_')[0];
            let idString = args[0];
            let typeIndex = this.typeToIndex[dataType];

            let prefabPath = this.targetFolder + category.toLowerCase() + '\\';
            if(!fs.existsSync(prefabPath)) {
                fs.mkdirSync(prefabPath);
            }

            let prefabFile = prefabPath + idString + '.tres';
            // console.log(prefabFile + ' || ' + idString + ' -- ' + typeIndex);

            let prefab = PrefabTemplate
                .replace('#ID_STRING#', idString)
                .replace('#TYPE_VAL#', typeIndex)
                .replace('#LINK_NAME#', 'LINK_' + idString);
            fs.writeFileSync(prefabFile, prefab);
        }
    }
}

let gen = new PrefabGenerator();
gen.execute();