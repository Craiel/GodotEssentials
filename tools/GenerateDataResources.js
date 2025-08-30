'use strict';

const fs = require('fs');
const path = require('path');
const EssentialsRoot = path.dirname(__dirname);

let ProjectFolder = process.argv[2];
if(ProjectFolder === undefined) {
    ProjectFolder = path.join("..", "Project");
}

let SourceFolder = process.argv[3];
if(SourceFolder === undefined) {
    SourceFolder = path.join(ProjectFolder, "source", "Game");
}

class PrefabGenerator {
    constructor() {
        this.results = {};
        this.filesToIndex = [];
        this.extensionsToIndex = ['.cs'];
        this.targetFolder = path.join(ProjectFolder, "prefabs", "database") + path.sep;
        this.sourceFolder = path.join(SourceFolder, "Database") + path.sep;
        this.dataTypeFile = path.join(EssentialsRoot, 'scripts', 'Database', 'GameDataType.cs');
        this.linkScriptUID = "uid://djka00sky8fkr";
        this.linkScript = 'res://source/GodotEssentials/scripts/Database/GameDatabaseLinkNode.cs';
        this.typeToIndex = {};
    }

    getRelativeAssetPath(filePath) {
        let pathRootIndex = filePath.indexOf(path.sep + 'Database' + path.sep);
        return filePath.substring(pathRootIndex + 10, filePath.length);
    }

    indexFile(filePath) {

        let pathStr = this.getRelativeAssetPath(filePath);
        let segments = pathStr.split(path.sep);
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


    buildGodotFile(idString, typeIndex){
        let resourceFile = require('./tools_src/godot_file.js').createNew('tres');

        let mainSection = resourceFile.addSection('gd_resource');
        mainSection.addQuotedParam('type', 'Resource');
        mainSection.addQuotedParam('script_class', 'GameDatabaseLinkNode');
        mainSection.addParam('load_steps', '2', 2);
        mainSection.addParam('format', '3', 2);

        let scriptId = resourceFile.getNewLocalId();
        let extResource = resourceFile.addSection('ext_resource');
        extResource.addQuotedParam('type', 'Script');
        extResource.addQuotedParam('uid', this.linkScriptUID);
        extResource.addQuotedParam('path', this.linkScript);
        extResource.addQuotedParam('id', scriptId);

        let resource = resourceFile.addSection('resource');
        resource.addContent('script', 'ExtResource("' + scriptId + '")');
        resource.addContent('Id', '"' + idString + '"');
        resource.addContent('Type', typeIndex.toString());

        return resourceFile;
    }

    writeResults() {
        fs.rmSync(this.targetFolder, { recursive: true, force: true });
        fs.mkdirSync(this.targetFolder);

        const idMatchRegex = /\snew\s*\((.+?)\);/;
        for(let id in this.results) {
            let file = this.results[id];
            let fileContents = fs.readFileSync(file).toString().split("\n");
            let match = null;
            for(let i = 0; i < fileContents.length; i++) {
                if(fileContents[i].indexOf('GameDataType.') < 0 || fileContents[i].indexOf("StringGameDataId") < 0) {
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

            let args = match[1].split(',');
            for(let i = 0; i < args.length; i++) {
                args[i] = args[i].trim().replaceAll('"', '');
            }

            let dataType = args[1].replace('GameDataType.', '');
            if(dataType === 'Unset') {
                continue;
            }

            let category = id.split('_')[0];
            let idString = args[0].replace('nameof(', '').replace(')', '');
            let typeIndex = this.typeToIndex[dataType];

            let prefabPath = path.join(this.targetFolder, category.toLowerCase()) + path.sep;
            if(!fs.existsSync(prefabPath)) {
                fs.mkdirSync(prefabPath);
            }

            let resourceFilePath = prefabPath + idString + '.tres';
            let resourceFile = this.buildGodotFile(idString, typeIndex);
            resourceFile.saveAs(resourceFilePath);
        }
    }
}

let gen = new PrefabGenerator();
gen.execute();