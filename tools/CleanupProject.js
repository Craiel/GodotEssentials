'use strict';

const fs = require('fs');
const path = require('path');
const EssentialsRoot = path.dirname(__dirname);

let SourceFolder = process.argv[2];
if(SourceFolder === undefined) {
    SourceFolder = path.join("..", "Project", "source", "Game");
}

class ProjectCleaner {
    constructor() {
        this.sourceFolder = SourceFolder;
    }

     execute() {
        console.log('Checking UID files in ' + this.sourceFolder);
        let checked = this.cleanUIDFiles(this.sourceFolder);
        console.log(" -> " + checked + " Files Checked\n");
    }

    cleanUIDFiles(dir) {
        let checked = 0;
        if(!fs.existsSync(dir)) {
            console.error('Directory not found: ' + dir);
            return 0;
        }

        let contents = fs.readdirSync(dir);
        for (let f = 0; f < contents.length; f++) {
            checked++;
            let fullPath = path.join(dir, contents[f]);
            let pathLStat = fs.lstatSync(fullPath);
            if(pathLStat.isDirectory() === true) {
                checked += this.cleanUIDFiles(fullPath);
            } else {
                if(fullPath.endsWith(".uid")) {
                    let csFile = fullPath.replace(".uid", "");
                    if(!fs.existsSync(csFile)) {
                        console.warn("Orphan UID File: " + fullPath);
                        fs.unlinkSync(fullPath);
                    }
                }
            }
        }

        return checked;
    }
}

var cleaner = new ProjectCleaner();
cleaner.sourceFolder = SourceFolder;
cleaner.execute();

cleaner = new ProjectCleaner();
cleaner.sourceFolder = EssentialsRoot;
cleaner.execute();