'use strict';

const fs = require("fs");
const path = require("path");

const FileSection = require('./godot_file_section.js');

const FileType = Object.freeze({
    Scene: 'tscn',
    Resource: 'tres'
});

class GodotFile {
    constructor(src, type) {
        this.filePath = src;

        if(src !== undefined) {
            this.fileName = path.basename(src);
            this.fileExtension = path.extname(src).replace('.', '');
        } else {
            this.fileType = type;
        }

        this.sections = [];

        for(let type in FileType) {
            if(this.fileExtension === FileType[type]) {
                this.fileType = type;
            }
        }

        if(this.fileType === undefined) {
            throw "Unknown file type: " + this.fileExtension;
        }

        // console.log(" -> " + this.fileType + ' | ' + src);
    }

    load() {
        if(!fs.existsSync(this.filePath)) {
            throw "Can not load, file does not exist: " + this.filePath;
        }

        let contents = fs.readFileSync(this.filePath, 'UTF8').split('\n');
        let activeSection = null;
        for(let i = 0; i < contents.length; i++) {
            let line = contents[i].trim().replaceAll('\r', '');
            if(line.length === 0) {
                continue;
            }

            if(line[0] === '[') {
                if(activeSection !== null) {
                    this.sections.push(activeSection);
                }

                activeSection = FileSection.createFromData(line);
                continue;
            }

            let valueSplit = line.indexOf('=');
            if(valueSplit > 0) {
                let valueKey = line.substring(0, valueSplit).trim();
                let value = line.substring(valueSplit + 1, line.length).trim();
                activeSection.addContent(valueKey, value);
                continue;
            }

            console.warn("Unhandled line: " + line + ' (' + this.fileName + ')');
        }

        if(activeSection !== null) {
            this.sections.push(activeSection);
        }
    }

    save() {
        this.saveAs(this.filePath);
    }

    addSection(id) {
        let section = FileSection.create(id);
        this.sections.push(section);
        return section;
    }

    saveAs(file) {
        let outData = [];
        for(let i = 0; i < this.sections.length; i++) {
            let section = this.sections[i];
            section.save(outData);
        }

        fs.writeFileSync(file, outData.join("\r\n"));
    }

    deleteSection(section) {
        let index = this.sections.indexOf(section);
        if(index < 0) {
            throw "Section does not exist";
        }

        this.sections.splice(index, 1);
    }

    getLastIndexOfSection(sectionId) {
        let index = 0;
        for(let i = 0; i < this.sections.length; i++) {
            let section = this.sections[i];
            if(section !== undefined && section.id !== sectionId) {
                continue;
            }

            index = i;
        }

        return index;
    }

    getSections(sectionId, paramKeyFilter, paramValueFilter) {
        if(paramValueFilter !== undefined && paramKeyFilter === undefined) {
            throw "Value filter requires Key Filter!";
        }

        let results = [];
        for(let i = 0; i < this.sections.length; i++) {
            let section = this.sections[i];
            if(section !== undefined && section.id !== sectionId) {
                continue;
            }

            if(paramKeyFilter !== undefined && !section.hasParam(paramKeyFilter)) {
                continue;
            }

            if(paramValueFilter !== undefined) {
                let paramValue = section.getParamValue(paramKeyFilter);
                if(paramValue !== paramValueFilter) {
                    continue;
                }
            }

            results.push(section);
        }

        return results;
    }

    getNewLocalId() {
        const id_base = (Math.random() + 1).toString(36).substring(7);
        const id_pre = Math.floor(Math.random() * 10);
        return id_pre + '_' + id_base;
    }
}

module.exports = {
    createNew: function(type) { return new GodotFile(undefined, type); },
    createFromFile: function(path) { 
        var file = new GodotFile(path);
        file.load();
        return file;
    },
    getResPath(filePath) {
        let result = filePath.replace('..\\Project\\', 'res://');
        result = result.replaceAll("\\", '/');
        return result;
    },
    GodotFile: GodotFile
};