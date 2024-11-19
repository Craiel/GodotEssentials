'use strict';

const fs = require('fs');
const path = require('path');

(function init() {
    let mode = process.argv[2];
    let tileFile = process.argv[3];
    if(tileFile === undefined || !fs.existsSync(tileFile)) {
        throw "Invalid Tileset File: " + file;
    }

    let file = process.argv[4];
    if(file === undefined) {
        throw "Missing input file name";
    }

    switch (mode) {
        case 'nav_extract': {
            const tileSet = require('./tools_src/tileset_file.js').create(tileFile);
            tileSet.load();
            tileSet.saveNavData(file);
            break;
        }

        case 'nav_apply': {
            const tileSet = require('./tools_src/tileset_file.js').create(tileFile);
            tileSet.load();
            tileSet.loadNavData(file);
            tileSet.save();
            break;
        }

        case 'phys_extract': {
            const tileSet = require('./tools_src/tileset_file.js').create(tileFile);
            tileSet.load();
            tileSet.savePhysicsData(file);
            break;
        }

        case 'phys_apply': {
            const tileSet = require('./tools_src/tileset_file.js').create(tileFile);
            tileSet.load();
            tileSet.loadPhysicsData(file);
            tileSet.save();
            break;
        }

        default: {
            throw "Mode not supported ";
        }
    }
})();