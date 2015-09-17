module.exports = function (grunt) {
    "use strict";
    var distRoot = "dist/";
    var tmpRoot = "tmp/";

    var sxcadmin = {
        cwd: "src/sxc-admin/",
        cwdJs: ["src/sxc-admin/**/*.js"],
        tmp: "tmp/sxc-admin/",
        templates: "tmp/sxc-admin/sxc-templates.js",
        dist: "dist/admin/",
        concatFile: "dist/admin/sxc-admin.js",
        uglifyFile: "dist/admin/sxc-admin.min.js"
    };
    var inpage = {
        cwd: "src/inpage/",
        cwdJs: ["src/inpage/**/*.js"],
        tmp: "tmp/inpage/",
        templates: "tmp/inpage/inpage-templates.js",
        dist: "dist/admin/",
        concatFile: "dist/inpage/inpage.js",
        uglifyFile: "dist/inpage/inpage.min.js"
    };
    var i18n = {
        cwd: "src/i18n/",
        dist: "dist/i18n/"
    };
    var sxc4ng = "js/AngularJS/2sxc4ng.js";



  // Project configuration.
    grunt.initConfig({
        pkg: grunt.file.readJSON("package.json"),

        jshint: {
            options: {
                laxbreak: true,
                scripturl: true
            },
            all: ["gruntfile.js", sxcadmin.cwd, inpage.cwd, sxc4ng],
            Sxc: ["js/2sxc.api.js", "js/2sxc.api.manage.js"]//, "js/template-selector/template-selector.js"],
        },

        clean: {
            tmp: tmpRoot + "**/*"
        },

        copy: {
            build: {
                files: [
                    {
                        expand: true,
                        cwd: sxcadmin.cwd,
                        src: ["**", "!**/*Spec.js"],
                        dest: sxcadmin.tmp
                    },
                    {
                        expand: true,
                        cwd: inpage.cwd,
                        src: ["**/*.*"],
                        dest: inpage.tmp
                    },
                    {
                        expand: true,
                        cwd: "src/dnn/",
                        src: ["**/*.*"],
                        dest: "dist/dnn/"
                    }
                ]
            },
            i18n: {
                files: [
                    {
                        expand: true,
                        cwd: "src/i18n/", 
                        src: ["**/*.json"],
                        dest: "dist/i18n/", 
                        rename: function (dest, src) {
                            return dest + src.replace(".json", ".js");
                        }
                    }

                ]
            }
        },


        ngtemplates: {
            default: {
                options: {
                    module: "SxcTemplates",
                    append: true,
                    htmlmin: {
                        collapseBooleanAttributes: true,
                        collapseWhitespace: true,
                        removeAttributeQuotes: true,
                        removeComments: true,
                        removeEmptyAttributes: true,
                        removeRedundantAttributes: false,
                        removeScriptTypeAttributes: true,
                        removeStyleLinkTypeAttributes: true
                    }
                },
                files: [
                    {
                        cwd: sxcadmin.tmp,
                        src: ["**/*.html"], 
                        dest: sxcadmin.templates
                    }
                ]
            },
            inpage: {
                options: {
                    module: "SxcInpageTemplates",
                    append: true,
                    htmlmin: {
                        collapseBooleanAttributes: true,
                        collapseWhitespace: true,
                        removeAttributeQuotes: true,
                        removeComments: true,
                        removeEmptyAttributes: true,
                        removeRedundantAttributes: false,
                        removeScriptTypeAttributes: true,
                        removeStyleLinkTypeAttributes: true
                    }
                },
                files: [
                    {
                        cwd: inpage.tmp,
                        src: ["**/*.html"],
                        dest: inpage.templates
                    }
                ]
            }
        },

        concat: {
            default: {
                src: sxcadmin.tmp + "**/*.js",
                dest: sxcadmin.concatFile
            },
            inpage: {
                src: inpage.tmp + "**/*.js",
                dest: inpage.concatFile
            }
        },


        ngAnnotate: {
            options: {
                // Task-specific options go here. 
                // disable sourceMap for now as we can't pass it through to uglify yet (don't know how) sourceMap: true
            },
            sxcadmin: {
                expand: true,
                src: sxcadmin.concatFile,
                extDot: "last"          // Extensions in filenames begin after the last dot 
            },
            inpage: {
                expand: true,
                src: inpage.concatFile,
                extDot: "last"          // Extensions in filenames begin after the last dot 
            },
            Sxc4ng: {
                files: {
                    'js/AngularJS/2sxc4ng.annotated.js': ["js/AngularJS/2sxc4ng.js"]
                }
            },
            //SxcModuleUi: {
            //        files: {
            //            'js/template-selector/template-selector.annotated.js': ["js/template-selector/template-selector.js"]
            //        }
            //}
        },

        uglify: {
            options: {
                banner: "/*! <%= pkg.name %> <%= grunt.template.today(\"yyyy-mm-dd\") %> */\n",
                sourceMap: true
            },
            sxcadmin: {
                src: sxcadmin.concatFile,
                dest: sxcadmin.uglifyFile
            },
            inpage: {
                src: inpage.concatFile,
                dest: inpage.uglifyFile
            },
            Sxc4ng: {
                files: {
                    'js/AngularJS/2sxc4ng.min.js': ["js/AngularJS/2sxc4ng.annotated.js"]
                }
            }, 
            SxcCore: {
                files: {
                    'js/2sxc.api.min.js': ["js/2sxc.api.js"],
                    'js/2sxc.api.manage.min.js': ["js/2sxc.api.manage.js"],
                    //'js/template-selector/template-selector.min.js': ["js/template-selector/template-selector.annotated.js"],
                    'js/dnn-inpage-edit.min.js': ["js/dnn-inpage-edit.js"]
                }
            },
            //SxcModuleUi: {
            //    files: {
            //        'js/template-selector/template-selector.min.js': ["js/template-selector/template-selector.annotated.js"]
            //    }

            //}
        },
        
        // not in use yet
        //cssmin: {
        //    options: {
        //        shorthandCompacting: false,
        //        roundingPrecision: -1
        //    },
        //    target: {
        //        files: [{
        //            expand: true,
        //            cwd: distRoot,
        //            src: ["**/*.css", "!**/*.min.css"],
        //            dest: distRoot,
        //            ext: ".min.css"
        //        }
        //        ]
        //    }
        //},

        compress: {
            main: {
                options: {
                    mode: "gzip"
                },
                expand: true,
                cwd: distRoot,
                src: ["**/*.min.js"],
                dest: distRoot,
                ext: ".gz.js"
            }
        },

        // note: jasmine not in use yet
        jasmine: {
            
        },
        

        watch: {
            sxcbuild: {
                files: ["gruntfile.js", "src/**"],
                tasks: ["build"]
            }
        }


    });

    grunt.loadNpmTasks("grunt-contrib-uglify");
    grunt.loadNpmTasks("grunt-contrib-jshint");
    grunt.loadNpmTasks("grunt-contrib-jasmine");
    grunt.loadNpmTasks("grunt-contrib-watch");
    grunt.loadNpmTasks("grunt-ng-annotate");
    grunt.loadNpmTasks("grunt-contrib-concat");
    grunt.loadNpmTasks("grunt-contrib-copy");
    grunt.loadNpmTasks("grunt-contrib-clean");
    grunt.loadNpmTasks("grunt-ng-templates");
    grunt.loadNpmTasks("grunt-contrib-compress");
    grunt.loadNpmTasks("grunt-contrib-cssmin");

    // Default task(s).
    grunt.registerTask("build", [
        "jshint",
        "clean:tmp",
        "copy",
        "ngtemplates",
        "concat",
        "ngAnnotate",
        "uglify",
        //"cssmin",
        //"clean:tmp",
        "watch:sxcbuild"

    ]);
  grunt.registerTask("default", ["jshint", "ngAnnotate", "uglify"]);

};