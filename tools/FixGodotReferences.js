'use strict';

const fs = require('fs');
const path = require('path');

const Root = '../Project/';

let extensionsToScan = ['.tscn', '.tres']
let filesToScan = [];

// Add custom file name remaps here, dictionary format "old_file_name.*": "new_file_name.*"
let fileNameRemap = {
}

let fileRegister = {};

function getGodotResPath(filePath) {
    let result = filePath.replace('..\\Project\\', 'res://');
    result = result.replaceAll("\\", '/');
    return result;
}

function scanFile(filePath) {
    let contents = fs.readFileSync(filePath, 'UTF8');
    let lines = contents.split('\n');
    let newLines = [];
    let fileNeededCorrections = false;
    for(let l = 0; l < lines.length; l++) {
        let line = lines[l];
        let match = line.match(/\[\s*ext_resource\s.*?path=\"([^\"]+)\".*?\]/);
        if(match === null){
            newLines.push(line);
            continue;
        }

        let resourceRef = match[1];
        let resourcePath = path.join(Root, resourceRef.replace("res://", ""));
        if(fs.existsSync(resourcePath) === true) {
            newLines.push(line);
            continue;
        }

        fileNeededCorrections = true;

        let resourceFileName = path.basename(resourceRef);
        if(fileNameRemap[resourceFileName] !== undefined) {
            console.log("REMAP: " + resourceFileName + " -> " + fileNameRemap[resourceFileName]);
            resourceFileName = fileNameRemap[resourceFileName];
        }

        if(fileRegister[resourceFileName] !== undefined) {
            let registeredPaths = fileRegister[resourceFileName];
            if(registeredPaths.length === 1) {
                let newPath = getGodotResPath(registeredPaths[0]);
                newLines.push(match[0].replace(match[1], newPath));
                continue;
            }

            console.log(registeredPaths);
            throw "MOVED_REF has multiple alternatives"
        }

        console.warn("MISSING_REF: " + resourceRef);
        newLines.push(line);
    }

    if(fileNeededCorrections !== true) {
        return;
    }

    // fs.renameSync(filePath, filePath + ".bak");
    fs.writeFileSync(filePath, newLines.join("\n"), 'UTF8');
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
            if(fileRegister[fileName] === undefined) {
                fileRegister[fileName] = [fullPath];
            } else {
                fileRegister[fileName].push(fullPath);
            }

            for(let i = 0; i < extensionsToScan.length; i++){
                if(fullPath.endsWith(extensionsToScan[i])){
                    filesToScan.push(fullPath);
                    break;
                }
            }
        }
    }
}


findFiles(Root);

console.log("Scanned " + foldersScanned + " Folders, found " + filesToScan.length + " Files to check");
for(let i = 0; i < filesToScan.length; i++) {
    scanFile(filesToScan[i]);
}