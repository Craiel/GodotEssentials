'use strict';

const fs = require('fs');
const path = require('path');
const gf = require('./tools_src/godot_file.js');
const EssentialsRoot = path.dirname(__dirname);

const Root = path.join('..', 'Project') + path.sep;

let extensionsToScan = ['.tscn', '.tres']
let filesToScan = [];

// Add custom file name remaps here, dictionary format "old_file_name.*": "new_file_name.*"
let fileNameRemap = {
}

let fileRegister = {};

function scanFile(filePath) {
    let file = gf.createFromFile(filePath);
    
    let scriptReferences = file.getSections('ext_resource', 'type', 'Script');

    let fileNeededCorrections = false;
    for(let i = 0; i < scriptReferences.length; i++) {
        let godotPath = scriptReferences[i].parameters.path.replace("res://", "");
        let filePath = path.join(Root, godotPath);
        if(fs.existsSync(filePath) === true) {
            continue;
        }

        fileNeededCorrections = true;
        let resourceFileName = path.basename(godotPath);
        if(fileNameRemap[resourceFileName] !== undefined) {
            console.log("REMAP: " + resourceFileName + " -> " + fileNameRemap[resourceFileName]);
        }
        
        if(fileRegister[resourceFileName] !== undefined) {
            let registeredPaths = fileRegister[resourceFileName];
            if(registeredPaths.length === 1) {
                // console.log(scriptReferences[i].parameters.path + " -> " + gf.getResPath(registeredPaths[0]));
                scriptReferences[i].parameters.path = gf.getResPath(registeredPaths[0]);
                continue;
            }

            console.log(registeredPaths);
            throw "MOVED_REF has multiple alternatives"
        }

        console.warn("MISSING_REF: " + resourceRef);
    }

    if(fileNeededCorrections !== true) {
        return;
    }

    // fs.renameSync(filePath, filePath + ".bak");
    // fs.writeFileSync(filePath, newLines.join("\n"), 'UTF8');
    file.save();
}

let foldersScanned = 0;
function findFiles(dir) {
    foldersScanned++;
    let contents = fs.readdirSync(dir);
    for (let f = 0; f < contents.length; f++) {
        let fileName = contents[f];
        let fullPath = path.join(dir, contents[f]);
        let pathLStat = fs.lstatSync(fullPath);
        if(pathLStat.isDirectory() === true || pathLStat.isSymbolicLink() === true) {
            findFiles(fullPath);
        } else {
            if(fullPath.indexOf(path.sep + 'GodotEssentials' + path.sep) >= 0) {
                continue;
            }    

            for(let i = 0; i < extensionsToScan.length; i++){
                if(fullPath.endsWith(extensionsToScan[i])){
                    filesToScan.push(fullPath);
                    break;
                }
            }                   

            if(!fullPath.endsWith('.cs')) {
                continue;
            }

            if(fileRegister[fileName] === undefined) {
                fileRegister[fileName] = [fullPath];
            } else {
                fileRegister[fileName].push(fullPath);
            }            
        }
    }
}


findFiles(Root);

console.log("Scanned " + foldersScanned + " Folders, found " + filesToScan.length + " Files to check");
for(let i = 0; i < filesToScan.length; i++) {
    scanFile(filesToScan[i]);
}