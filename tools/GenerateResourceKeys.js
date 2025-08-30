'use strict';

const fs = require('fs');
const path = require('path');
const EssentialsRoot = path.dirname(__dirname);

let SourceFolder = process.argv[2];
if(SourceFolder === undefined) {
    SourceFolder = path.join("..", "Project", "source", "Game");
}

let NameSpace = process.argv[3];
if(NameSpace === undefined) {
    NameSpace = "undefined";
}

class ResourceIndexer {
    constructor() {
        this.results = {};
        this.filesToIndex = [];
        this.extensionsToIndex = ['.png'];
        this.targetFolder = path.join(SourceFolder, "Database") + path.sep;
        this.sourceFolder = path.join("..", "Project", "art") + path.sep;
        this.targetFile = "ArtResources.cs";
        this.category = "art";
        this.className = "ArtResources";
        this.assetType = "Texture2D";
        this.resourcePrefixPath = "";
    }

    getGodotResPath(filePath) {
        let result = "res://" + this.resourcePrefixPath + this.category + "/" + filePath.replaceAll(path.sep, "/");
        return result;
    }

    indexFile(filePath) {
        let pathStr = filePath.replace(this.sourceFolder, "");
        let segments = pathStr.split(path.sep);
        let id = "";
        for(let i = 0; i < segments.length; i++) {
            if(i === 0 && segments.length > 1) {
                id = segments[i];
                continue;
            }

            let segmentFormatted = segments[i].replaceAll(/[^a-z0-9\.]/gi, " ").trim().replaceAll(/[\s]+/g, "_");
            if(i == segments.length - 1) {
                id = id + "_" + segmentFormatted;
                continue;
            }

            id = id + "_" + segmentFormatted;
        }

        for(let i = 0; i < this.extensionsToIndex.length; i++) {
            id = id.replace(this.extensionsToIndex[i], "");
        }

        id = id.toUpperCase();
        pathStr = this.getGodotResPath(pathStr);

        if(this.results[id] !== undefined) {
            
            console.log(segments);
            throw "Duplicate ID: " + id + " -> " + this.results[id];
        }

        this.results[id] = pathStr;
    }
    
    execute() {
        this.findFiles(this.sourceFolder);
        for(let i = 0; i < this.filesToIndex.length; i++) {
            this.indexFile(this.filesToIndex[i]);
        }

        this.writeResults();
    }

    findFiles(dir) {
        if(!fs.existsSync(dir)) {
            return;
        }
        
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

    writeResults() {
        let contents = [];
        contents.push("namespace " + NameSpace + ".Core;\n");
        contents.push("using Craiel.Essentials.Resource;");
        contents.push("using Craiel.Essentials.Utils;");
        contents.push("using Godot;\n");
        contents.push("// ReSharper disable InconsistentNaming");
        contents.push("public static class "+ this.className + "\n{");
        let currentCategory = undefined;
        for(let id in this.results) {
            let segments = id.split('_');
            let category = segments[0];

            if(currentCategory === undefined || category !== currentCategory) {
                if(currentCategory !== undefined) {
                    contents.push("");
                }

                currentCategory = category;
                contents.push("    // " + category);
            }

            let entry = "    public static readonly ResourceKey " + id + " = new ResourceKey(\"" + this.results[id] + "\", TypeDef<" + this.assetType + ">.Value);";
            contents.push(entry);
        }

        contents.push("}");
        contents.push("// ReSharper enable InconsistentNaming");

        let outFile = this.targetFolder + this.targetFile;
        console.log(outFile);
        fs.writeFileSync(outFile, contents.join("\n"), 'UTF8');
    }
}

var essArtClass = new ResourceIndexer();
essArtClass.sourceFolder = path.join(EssentialsRoot, "data", "art") + path.sep;
essArtClass.targetFile = "EssentialArtResources.cs";
essArtClass.className = "EssentialArtResources";
essArtClass.resourcePrefixPath = "source/GodotEssentials/data/";
essArtClass.execute();

var artClass = new ResourceIndexer();
artClass.execute();

var soundClass = new ResourceIndexer();
soundClass.extensionsToIndex = ['.wav', '.ogg', '.mp3'];
soundClass.sourceFolder = path.join("..", "Project", "sound") + path.sep;
soundClass.targetFile = "SoundResources.cs";
soundClass.category = "sound";
soundClass.className = "SoundResources";
soundClass.assetType = "AudioStream";
soundClass.execute();