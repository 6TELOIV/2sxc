(window.webpackJsonp=window.webpackJsonp||[]).push([[15],{"25Tt":function(e,t,n){"use strict";n.r(t);var a=n("5/c3"),i=n("LR82"),o=n("z5yO"),s=n("1C3z"),r=n("BLjT"),c=n("Iv+g"),l=n("9HSk"),p=n("Qc/f"),u=n("r4gC"),b=n("JNB8"),d=n("KLQV"),m=n("8AiQ");function h(e,t){1&e&&(s.Wb(0,"mat-icon",3),s.Pc(1,"visibility"),s.Vb())}function f(e,t){1&e&&(s.Wb(0,"mat-icon",4),s.Pc(1,"visibility_off"),s.Vb())}var g=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-apps-list-show"]],decls:3,vars:2,consts:[[1,"icon-container"],["matTooltip","Show this app to users",4,"ngIf"],["matTooltip","Don't show this app to users",4,"ngIf"],["matTooltip","Show this app to users"],["matTooltip","Don't show this app to users"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Nc(1,h,2,0,"mat-icon",1),s.Nc(2,f,2,0,"mat-icon",2),s.Vb()),2&e&&(s.Bb(1),s.qc("ngIf",t.value),s.Bb(1),s.qc("ngIf",!t.value))},directives:[m.t,u.a,p.a],styles:[""]}),e}(),v=n("OeRG");function C(e,t){if(1&e){var n=s.Xb();s.Wb(0,"div",3),s.ec("click",(function(){return s.Ec(n),s.ic().deleteApp()})),s.Wb(1,"mat-icon"),s.Pc(2,"delete"),s.Vb(),s.Vb()}}function y(e,t){1&e&&(s.Wb(0,"div",4),s.Wb(1,"mat-icon"),s.Pc(2,"delete"),s.Vb(),s.Vb())}var w=function(){function e(){}return e.prototype.agInit=function(e){this.params=e,this.app=e.data},e.prototype.refresh=function(e){return!0},e.prototype.deleteApp=function(){this.params.onDelete(this.app)},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-apps-list-actions"]],decls:3,vars:2,consts:[[1,"actions-component"],["class","like-button highlight","matRipple","","matTooltip","Delete",3,"click",4,"ngIf"],["class","like-button disabled",4,"ngIf"],["matRipple","","matTooltip","Delete",1,"like-button","highlight",3,"click"],[1,"like-button","disabled"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Nc(1,C,3,0,"div",1),s.Nc(2,y,3,0,"div",2),s.Vb()),2&e&&(s.Bb(1),s.qc("ngIf",t.app.IsApp),s.Bb(1),s.qc("ngIf",!t.app.IsApp))},directives:[m.t,v.q,p.a,u.a],styles:[""]}),e}(),W=/^[A-Za-z](?:[A-Za-z0-9\s\(\)]+)*$/,V=n("CeOT"),x=n("JzAw"),I=n("2aC0"),T=n("2pW/"),L=n("G6Ml"),P=n("KYsL"),k=function(){function e(e,t,n,a){this.router=e,this.route=t,this.appsListService=n,this.snackBar=a,this.columnDefs=[{headerName:"ID",field:"Id",width:70,headerClass:"dense",cellClass:"id-action no-padding no-outline",cellRenderer:"idFieldComponent",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.idValueGetter},{headerName:"Show",field:"IsHidden",width:70,headerClass:"dense",cellClass:"icons no-outline",sortable:!0,filter:"booleanFilterComponent",cellRenderer:"appsListShowComponent",valueGetter:this.showValueGetter},{headerName:"Name",field:"Name",flex:2,minWidth:250,cellClass:"primary-action highlight",sortable:!0,filter:"agTextColumnFilter",onCellClicked:this.openApp.bind(this)},{width:40,cellClass:"secondary-action no-padding",cellRenderer:"appsListActionsComponent",cellRendererParams:{onDelete:this.deleteApp.bind(this)}},{headerName:"Folder",field:"Folder",flex:2,minWidth:250,cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter"}],this.frameworkComponents={booleanFilterComponent:V.a,idFieldComponent:x.a,appsListShowComponent:g,appsListActionsComponent:w},this.modules=d.a,this.subscription=new i.a,this.hasChild=!!this.route.snapshot.firstChild.firstChild}return e.prototype.ngOnInit=function(){this.fetchAppsList(),this.refreshOnChildClosed()},e.prototype.ngOnDestroy=function(){this.subscription.unsubscribe(),this.subscription=null},e.prototype.browseCatalog=function(){window.open("http://2sxc.org/apps")},e.prototype.createApp=function(){var e=this,t=prompt("Enter App Name (will also be used for folder)");if(null!==t){for(t=t.trim().replace(/\s\s+/g," ");!t.match(W);){if(null===(t=prompt("Enter App Name (will also be used for folder)\nStandard letters, numbers, spaces and round brackets are allowed. Must start with a letter.",t)))return;t=t.trim().replace(/\s\s+/g," ")}this.snackBar.open("Saving..."),this.appsListService.create(t).subscribe((function(){e.snackBar.open("Saved",null,{duration:2e3}),e.fetchAppsList()}))}},e.prototype.importApp=function(){this.router.navigate(["import"],{relativeTo:this.route.firstChild})},e.prototype.fetchAppsList=function(){var e=this;this.appsListService.getAll().subscribe((function(t){e.apps=t}))},e.prototype.idValueGetter=function(e){var t=e.data;return"ID: "+t.Id+"\nGUID: "+t.Guid},e.prototype.showValueGetter=function(e){return!e.data.IsHidden},e.prototype.deleteApp=function(e){var t=this,n=prompt("This cannot be undone. To really delete this app, type 'yes!' or type/paste the app-name here. Are you sure want to delete '"+e.Name+"' ("+e.Id+")?");null!==n&&(n===e.Name||"yes!"===n?(this.snackBar.open("Deleting..."),this.appsListService.delete(e.Id).subscribe((function(){t.snackBar.open("Deleted",null,{duration:2e3}),t.fetchAppsList()}))):alert("Input did not match - will not delete"))},e.prototype.openApp=function(e){this.router.navigate([e.data.Id.toString()],{relativeTo:this.route.parent})},e.prototype.refreshOnChildClosed=function(){var e=this;this.subscription.add(this.router.events.pipe(Object(o.a)((function(e){return e instanceof a.b}))).subscribe((function(t){var n=e.hasChild;e.hasChild=!!e.route.snapshot.firstChild.firstChild,!e.hasChild&&n&&e.fetchAppsList()})))},e.\u0275fac=function(t){return new(t||e)(s.Qb(a.c),s.Qb(a.a),s.Qb(I.a),s.Qb(T.b))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-apps-list"]],decls:17,vars:10,consts:[[1,"grid-wrapper-nav"],[1,"ag-theme-material",3,"rowData","columnDefs","modules","frameworkComponents","enableCellTextSelection","suppressScrollOnNewData","animateRows","accentedSort","valueCache","headerHeight"],[1,"grid-fab"],["spin","true"],["mat-fab",""],[1,"spin180"],["mat-mini-fab","","matTooltip","Find more apps",3,"click"],["mat-mini-fab","","matTooltip","Import app",3,"click"],["mat-mini-fab","","matTooltip","Create app",3,"click"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Rb(1,"ag-grid-angular",1),s.Wb(2,"eco-fab-speed-dial",2),s.Wb(3,"eco-fab-speed-dial-trigger",3),s.Wb(4,"button",4),s.Wb(5,"mat-icon",5),s.Pc(6,"add"),s.Vb(),s.Vb(),s.Vb(),s.Wb(7,"eco-fab-speed-dial-actions"),s.Wb(8,"button",6),s.ec("click",(function(){return t.browseCatalog()})),s.Wb(9,"mat-icon"),s.Pc(10,"search"),s.Vb(),s.Vb(),s.Wb(11,"button",7),s.ec("click",(function(){return t.importApp()})),s.Wb(12,"mat-icon"),s.Pc(13,"cloud_upload"),s.Vb(),s.Vb(),s.Wb(14,"button",8),s.ec("click",(function(){return t.createApp()})),s.Wb(15,"mat-icon"),s.Pc(16,"add"),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Vb()),2&e&&(s.Bb(1),s.qc("rowData",t.apps)("columnDefs",t.columnDefs)("modules",t.modules)("frameworkComponents",t.frameworkComponents)("enableCellTextSelection",!0)("suppressScrollOnNewData",!0)("animateRows",!0)("accentedSort",!0)("valueCache",!0)("headerHeight",32))},directives:[L.a,P.b,P.d,l.b,u.a,P.a,p.a],styles:["ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 245px)}@media (max-width:600px){ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 215px)}}"]}),e}(),D=n("F7GT"),N=function(){function e(){}return e.prototype.agInit=function(e){this.params=e,this.value=e.value},e.prototype.refresh=function(e){return!0},e.prototype.toggleLanguage=function(){this.params.onEnabledToggle(this.params.data)},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-enable-languages-status"]],decls:2,vars:1,consts:[[1,"cell-box"],[3,"checked","change"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Wb(1,"mat-slide-toggle",1),s.ec("change",(function(){return t.toggleLanguage()})),s.Vb(),s.Vb()),2&e&&(s.Bb(1),s.qc("checked",t.value))},directives:[D.a],styles:[".cell-box[_ngcontent-%COMP%]{padding-left:8px;padding-right:8px}"]}),e}(),S=n("xNdR"),M=function(){function e(e){this.languagesService=e,this.columnDefs=[{headerName:"ID",field:"Code",width:70,headerClass:"dense",cellClass:"id-action no-padding no-outline",cellRenderer:"idFieldComponent",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.idValueGetter},{headerName:"Name",field:"Culture",flex:2,minWidth:250,cellClass:"primary-action highlight no-outline",sortable:!0,filter:"agTextColumnFilter",onCellClicked:this.handleNameClicked.bind(this)},{headerName:"Status",field:"IsEnabled",width:72,headerClass:"dense",cellClass:"no-padding no-outline",cellRenderer:"enableLanguagesStatusComponent",sortable:!0,filter:"booleanFilterComponent",cellRendererParams:{onEnabledToggle:this.toggleLanguage.bind(this)}}],this.frameworkComponents={idFieldComponent:x.a,booleanFilterComponent:V.a,enableLanguagesStatusComponent:N},this.modules=d.a}return e.prototype.ngOnInit=function(){this.fetchLanguages()},e.prototype.idValueGetter=function(e){return"ID: "+e.data.Code},e.prototype.handleNameClicked=function(e){this.toggleLanguage(e.data)},e.prototype.toggleLanguage=function(e){var t=this;this.languagesService.save(e.Code,!e.IsEnabled).subscribe((function(){t.fetchLanguages()}))},e.prototype.fetchLanguages=function(){var e=this;this.languagesService.getAll().subscribe((function(t){e.languages=t}))},e.\u0275fac=function(t){return new(t||e)(s.Qb(S.a))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-enable-languages"]],decls:2,vars:10,consts:[[1,"grid-wrapper-nav"],[1,"ag-theme-material",3,"rowData","columnDefs","modules","frameworkComponents","enableCellTextSelection","suppressScrollOnNewData","animateRows","accentedSort","valueCache","headerHeight"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Rb(1,"ag-grid-angular",1),s.Vb()),2&e&&(s.Bb(1),s.qc("rowData",t.languages)("columnDefs",t.columnDefs)("modules",t.modules)("frameworkComponents",t.frameworkComponents)("enableCellTextSelection",!0)("suppressScrollOnNewData",!0)("animateRows",!0)("accentedSort",!0)("valueCache",!0)("headerHeight",32))},directives:[L.a],styles:["ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 245px)}@media (max-width:600px){ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 215px)}}"]}),e}();function O(e,t){1&e&&(s.Wb(0,"mat-icon",3),s.Pc(1,"toggle_on"),s.Vb())}function R(e,t){1&e&&(s.Wb(0,"mat-icon",4),s.Pc(1,"toggle_off"),s.Vb())}var A=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-features-list-enabled"]],decls:3,vars:2,consts:[[1,"icon-container"],["matTooltip","Feature is enabled",4,"ngIf"],["matTooltip","Feature is disabled",4,"ngIf"],["matTooltip","Feature is enabled"],["matTooltip","Feature is disabled"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Nc(1,O,2,0,"mat-icon",1),s.Nc(2,R,2,0,"mat-icon",2),s.Vb()),2&e&&(s.Bb(1),s.qc("ngIf",t.value),s.Bb(1),s.qc("ngIf",!t.value))},directives:[m.t,u.a,p.a],styles:[""]}),e}();function F(e,t){1&e&&(s.Wb(0,"mat-icon"),s.Pc(1,"visibility"),s.Vb())}function B(e,t){1&e&&(s.Wb(0,"mat-icon"),s.Pc(1,"remove"),s.Vb())}var q=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-features-list-ui"]],decls:3,vars:2,consts:[[1,"icon-container"],[4,"ngIf"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Nc(1,F,2,0,"mat-icon",1),s.Nc(2,B,2,0,"mat-icon",1),s.Vb()),2&e&&(s.Bb(1),s.qc("ngIf",t.value),s.Bb(1),s.qc("ngIf",!t.value))},directives:[m.t,u.a],styles:[""]}),e}();function G(e,t){1&e&&(s.Wb(0,"mat-icon"),s.Pc(1,"person"),s.Vb())}function _(e,t){1&e&&(s.Wb(0,"mat-icon"),s.Pc(1,"remove"),s.Vb())}var E=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-features-list-public"]],decls:3,vars:2,consts:[[1,"icon-container"],[4,"ngIf"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Nc(1,G,2,0,"mat-icon",1),s.Nc(2,_,2,0,"mat-icon",1),s.Vb()),2&e&&(s.Bb(1),s.qc("ngIf",t.value),s.Bb(1),s.qc("ngIf",!t.value))},directives:[m.t,u.a],styles:[""]}),e}(),Q=function(){function e(){}return e.prototype.agInit=function(e){},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=s.Kb({type:e,selectors:[["app-features-list-security"]],decls:3,vars:0,consts:[[1,"icon-container"],["matTooltip","Security Status still work-in progress",1,"feature-security-icon"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Wb(1,"mat-icon",1),s.Pc(2,"help"),s.Vb(),s.Vb())},directives:[u.a,p.a],styles:[".feature-security-icon[_ngcontent-%COMP%]{color:green}"]}),e}(),z=n("Ata6"),U=n("ycnj"),K=n("nYrE");function H(e,t){1&e&&s.Rb(0,"mat-spinner",5)}function j(e,t){if(1&e&&s.Rb(0,"iframe",6),2&e){var n=s.ic();s.qc("src",n.managementUrl,s.Gc)}}var J=function(e){return{"iframe-wrapper":e}},Z=function(e){return{"force-hide":e}},Y=function(){function e(e,t){this.sanitizer=e,this.featuresConfigService=t,this.showManagement=!1,this.showSpinner=!1,this.columnDefs=[{headerName:"ID",field:"id",width:70,headerClass:"dense",cellClass:"id-action no-padding no-outline",cellRenderer:"idFieldComponent",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.idValueGetter},{headerName:"Enabled",field:"enabled",width:80,headerClass:"dense",cellClass:"no-outline",sortable:!0,filter:"booleanFilterComponent",cellRenderer:"featuresListEnabledComponent"},{headerName:"UI",field:"ui",width:70,headerClass:"dense",cellClass:"no-outline",sortable:!0,filter:"booleanFilterComponent",cellRenderer:"featuresListUiComponent"},{headerName:"Public",field:"public",width:70,headerClass:"dense",cellClass:"no-outline",sortable:!0,filter:"booleanFilterComponent",cellRenderer:"featuresListPublicComponent"},{headerName:"Name",field:"id",flex:2,minWidth:250,cellClass:"primary-action highlight",sortable:!0,filter:"agTextColumnFilter",onCellClicked:this.openFeature,cellRenderer:function(e){return"details (name lookup still WIP)"}},{headerName:"Expires",field:"expires",flex:1,minWidth:200,cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter",valueGetter:this.valueGetterDateTime},{headerName:"Security",width:70,cellClass:"no-outline",cellRenderer:"featuresListSecurityComponent"}],this.frameworkComponents={booleanFilterComponent:V.a,idFieldComponent:x.a,featuresListEnabledComponent:A,featuresListUiComponent:q,featuresListPublicComponent:E,featuresListSecurityComponent:Q},this.modules=d.a}return e.prototype.ngOnInit=function(){this.fetchFeatures()},e.prototype.ngOnDestroy=function(){this.destroyManagementListener()},e.prototype.toggleManagement=function(){this.showManagement=!this.showManagement,this.destroyManagementListener(),this.showManagement&&this.openManagement()},e.prototype.idValueGetter=function(e){return"GUID: "+e.data.id},e.prototype.openFeature=function(e){window.open("https://2sxc.org/r/f/"+e.value,"_blank")},e.prototype.fetchFeatures=function(){var e=this;this.featuresConfigService.getAll().subscribe((function(t){e.features=t}))},e.prototype.openManagement=function(){var e=this;this.showSpinner=!0,this.managementUrl=this.sanitizer.bypassSecurityTrustResourceUrl(""),this.featuresConfigService.getManageFeaturesUrl().subscribe((function(t){if(e.showSpinner=!1,t.indexOf("error: user needs host permissions")>-1)throw e.showManagement=!1,new Error("User needs host permissions!");e.managementUrl=e.sanitizer.bypassSecurityTrustResourceUrl(t);var n=e.managementCallback.bind(e);window.addEventListener("message",n),e.managementListener={element:window,type:"message",listener:n}}))},e.prototype.managementCallback=function(e){var t=this;if(this.destroyManagementListener(),void 0!==e.data&&!1!==e.origin.endsWith("2sxc.org"))try{var n=JSON.stringify(e.data);this.featuresConfigService.saveFeatures(n).subscribe((function(e){t.showManagement=!1,t.fetchFeatures()}))}catch(a){}},e.prototype.destroyManagementListener=function(){this.managementListener&&(this.managementListener.element.removeEventListener(this.managementListener.type,this.managementListener.listener),this.managementListener=null)},e.prototype.valueGetterDateTime=function(e){var t=e.data[e.colDef.field];return t?t.substring(0,19).replace("T"," "):null},e.\u0275fac=function(t){return new(t||e)(s.Qb(z.b),s.Qb(U.a))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-manage-features"]],decls:7,vars:18,consts:[[1,"grid-wrapper-nav",3,"ngClass"],[1,"ag-theme-material",3,"ngClass","rowData","columnDefs","modules","frameworkComponents","enableCellTextSelection","suppressScrollOnNewData","animateRows","accentedSort","valueCache","headerHeight"],["class","spinner","mode","indeterminate","diameter","20","color","accent",4,"ngIf"],["class","iframe",3,"src",4,"ngIf"],["mat-fab","","mat-elevation-z24","","matTooltip","Manage features",1,"grid-fab",3,"click"],["mode","indeterminate","diameter","20","color","accent",1,"spinner"],[1,"iframe",3,"src"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Rb(1,"ag-grid-angular",1),s.Nc(2,H,1,0,"mat-spinner",2),s.Nc(3,j,1,1,"iframe",3),s.Wb(4,"button",4),s.ec("click",(function(){return t.toggleManagement()})),s.Wb(5,"mat-icon"),s.Pc(6,"tune"),s.Vb(),s.Vb(),s.Vb()),2&e&&(s.qc("ngClass",s.vc(14,J,t.showManagement)),s.Bb(1),s.qc("ngClass",s.vc(16,Z,t.showManagement))("rowData",t.features)("columnDefs",t.columnDefs)("modules",t.modules)("frameworkComponents",t.frameworkComponents)("enableCellTextSelection",!0)("suppressScrollOnNewData",!0)("animateRows",!0)("accentedSort",!0)("valueCache",!0)("headerHeight",32),s.Bb(1),s.qc("ngIf",t.showSpinner),s.Bb(1),s.qc("ngIf",t.showManagement))},directives:[m.q,L.a,m.t,l.b,p.a,u.a,K.c],styles:[".iframe-wrapper[_ngcontent-%COMP%]{padding-top:20px}ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 245px)}@media (max-width:600px){ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 215px)}}.spinner[_ngcontent-%COMP%]{position:absolute;top:40px;left:20px}.iframe[_ngcontent-%COMP%]{border:none;width:100%;height:calc(100vh - 229px)}@media (max-width:600px){.iframe[_ngcontent-%COMP%]{height:calc(100vh - 199px)}}"]}),e}(),$=n("I3IT"),X=n("Bata"),ee=n("hOvr"),te=n("TDrE"),ne=n("ZSGP"),ae=n("Uk43");function ie(e,t){1&e&&(s.Wb(0,"app-field-hint",13),s.Pc(1,"Only positive whole numbers"),s.Vb()),2&e&&s.qc("isError",!0)}function oe(e,t){if(1&e&&(s.Ub(0),s.Nc(1,ie,2,1,"app-field-hint",12),s.Tb()),2&e){s.ic();var n=s.Cc(27);s.Bb(1),s.qc("ngIf",n.errors.pattern)}}var se=function(){function e(e,t){this.sxcInsightsService=e,this.snackBar=t,this.positiveWholeNumber=/^[^-]\d*$/,this.actionsDiabled=!1}return e.prototype.ngOnInit=function(){},e.prototype.openInsights=function(){window.open("/desktopmodules/2sxc/api/sys/insights/help")},e.prototype.activatePageLog=function(){var e=this;this.actionsDiabled=!0,this.snackBar.open("Activating..."),this.sxcInsightsService.activatePageLog(this.pageLogDuration).subscribe((function(t){e.pageLogDuration=void 0,e.actionsDiabled=!1,e.snackBar.open(t,null,{duration:4e3})}))},e.\u0275fac=function(t){return new(t||e)(s.Qb($.a),s.Qb(T.b))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-sxc-insights"]],decls:32,vars:4,consts:[[1,"cards-box"],[1,"mat-elevation-z2"],[1,"actions-box"],["mat-icon-button","","matTooltip","Open 2sxc Insights",3,"click"],["href","https://2sxc.org/en/blog/post/using-2sxc-insights","target","_blank"],[1,"activate-log-form"],["appearance","standard","color","accent"],["matInput","","type","number","min","0","name","Duration",3,"pattern","ngModel","ngModelChange"],["duration","ngModel"],[4,"ngIf"],[1,"actions"],["mat-raised-button","","color","accent",3,"disabled","click"],[3,"isError",4,"ngIf"],[3,"isError"]],template:function(e,t){if(1&e&&(s.Wb(0,"div",0),s.Wb(1,"mat-card",1),s.Wb(2,"mat-card-header"),s.Wb(3,"mat-card-title"),s.Pc(4,"2sxc Insights for Super Users"),s.Vb(),s.Wb(5,"div",2),s.Wb(6,"button",3),s.ec("click",(function(){return t.openInsights()})),s.Wb(7,"mat-icon"),s.Pc(8,"speed"),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Wb(9,"mat-card-content"),s.Pc(10," This is to access a special section to see what is really in the server memory. It's intended for extensive debugging - see also "),s.Wb(11,"a",4),s.Pc(12,"this blog post"),s.Vb(),s.Pc(13,". "),s.Vb(),s.Vb(),s.Wb(14,"mat-card",1),s.Wb(15,"mat-card-header"),s.Wb(16,"mat-card-title"),s.Pc(17,"Activate Page Level Logging"),s.Vb(),s.Rb(18,"div",2),s.Vb(),s.Wb(19,"mat-card-content"),s.Wb(20,"div"),s.Pc(21," This will place insights-logs in the HTML of the user output for users with ?debug=true in the url. It can only be activated for short periods of time. "),s.Vb(),s.Wb(22,"div",5),s.Wb(23,"mat-form-field",6),s.Wb(24,"mat-label"),s.Pc(25,"Duration in Minutes"),s.Vb(),s.Wb(26,"input",7,8),s.ec("ngModelChange",(function(e){return t.pageLogDuration=e})),s.Vb(),s.Vb(),s.Nc(28,oe,2,1,"ng-container",9),s.Wb(29,"div",10),s.Wb(30,"button",11),s.ec("click",(function(){return t.activatePageLog()})),s.Pc(31," Activate "),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Vb()),2&e){var n=s.Cc(27);s.Bb(26),s.qc("pattern",t.positiveWholeNumber)("ngModel",t.pageLogDuration),s.Bb(2),s.qc("ngIf",n.touched&&n.errors),s.Bb(2),s.qc("disabled",t.actionsDiabled||!t.pageLogDuration||t.pageLogDuration<0)}},directives:[X.a,X.f,X.m,l.b,p.a,u.a,X.d,ee.c,ee.g,te.b,ne.w,ne.c,ne.x,ne.r,ne.u,m.t,ae.a],styles:[".activate-log-form[_ngcontent-%COMP%]   .mat-form-field[_ngcontent-%COMP%]{height:auto}.activate-log-form[_ngcontent-%COMP%]   .actions[_ngcontent-%COMP%]{margin-top:8px;display:flex;justify-content:flex-end}"]}),e}();function re(e,t){1&e&&(s.Wb(0,"div",9),s.Wb(1,"mat-icon"),s.Pc(2,"star_border"),s.Vb(),s.Wb(3,"span"),s.Pc(4,"Apps"),s.Vb(),s.Vb())}function ce(e,t){1&e&&s.Rb(0,"app-apps-list")}function le(e,t){1&e&&(s.Wb(0,"div",10),s.Wb(1,"mat-icon"),s.Pc(2,"translate"),s.Vb(),s.Wb(3,"span"),s.Pc(4,"Languages"),s.Vb(),s.Vb())}function pe(e,t){1&e&&s.Rb(0,"app-enable-languages")}function ue(e,t){1&e&&(s.Wb(0,"div",11),s.Wb(1,"mat-icon"),s.Pc(2,"tune"),s.Vb(),s.Wb(3,"span"),s.Pc(4,"Features"),s.Vb(),s.Vb())}function be(e,t){1&e&&s.Rb(0,"app-manage-features")}function de(e,t){1&e&&(s.Wb(0,"div",12),s.Wb(1,"mat-icon"),s.Pc(2,"speed"),s.Vb(),s.Wb(3,"span"),s.Pc(4,"2sxc Insights"),s.Vb(),s.Vb())}function me(e,t){1&e&&s.Rb(0,"app-sxc-insights")}n.d(t,"AppsManagementNavComponent",(function(){return he}));var he=function(){function e(e,t,n,a){this.dialogRef=e,this.router=t,this.route=n,this.context=a,this.tabs=["list","languages","features","sxc-insights"],this.subscription=new i.a,this.zoneId=this.context.zoneId}return e.prototype.ngOnInit=function(){var e=this;this.tabIndex=this.tabs.indexOf(this.route.snapshot.firstChild.url[0].path),this.subscription.add(this.router.events.pipe(Object(o.a)((function(e){return e instanceof a.b}))).subscribe((function(t){e.tabIndex=e.tabs.indexOf(e.route.snapshot.firstChild.url[0].path)})))},e.prototype.ngOnDestroy=function(){this.subscription.unsubscribe(),this.subscription=null},e.prototype.closeDialog=function(){this.dialogRef.close()},e.prototype.changeTab=function(e){this.router.navigate([this.tabs[e.index]],{relativeTo:this.route})},e.\u0275fac=function(t){return new(t||e)(s.Qb(r.h),s.Qb(a.c),s.Qb(a.a),s.Qb(c.a))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-apps-management-nav"]],decls:21,vars:2,consts:[["mat-dialog-title",""],[1,"dialog-title-box"],["mat-icon-button","","matTooltip","Close dialog",3,"click"],["dynamicHeight","","color","accent",3,"selectedIndex","selectedTabChange"],["class","mat-tab-label-box","matTooltip","Apps",4,"matTabLabel"],[4,"matTabContent"],["class","mat-tab-label-box","matTooltip","Languages",4,"matTabLabel"],["class","mat-tab-label-box","matTooltip","These settings apply to all zones/portals",4,"matTabLabel"],["class","mat-tab-label-box","matTooltip","Insights",4,"matTabLabel"],["matTooltip","Apps",1,"mat-tab-label-box"],["matTooltip","Languages",1,"mat-tab-label-box"],["matTooltip","These settings apply to all zones/portals",1,"mat-tab-label-box"],["matTooltip","Insights",1,"mat-tab-label-box"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Wb(1,"div",1),s.Wb(2,"div"),s.Pc(3),s.Vb(),s.Wb(4,"button",2),s.ec("click",(function(){return t.closeDialog()})),s.Wb(5,"mat-icon"),s.Pc(6,"close"),s.Vb(),s.Vb(),s.Vb(),s.Vb(),s.Rb(7,"router-outlet"),s.Wb(8,"mat-tab-group",3),s.ec("selectedTabChange",(function(e){return t.changeTab(e)})),s.Wb(9,"mat-tab"),s.Nc(10,re,5,0,"div",4),s.Nc(11,ce,1,0,"app-apps-list",5),s.Vb(),s.Wb(12,"mat-tab"),s.Nc(13,le,5,0,"div",6),s.Nc(14,pe,1,0,"app-enable-languages",5),s.Vb(),s.Wb(15,"mat-tab"),s.Nc(16,ue,5,0,"div",7),s.Nc(17,be,1,0,"app-manage-features",5),s.Vb(),s.Wb(18,"mat-tab"),s.Nc(19,de,5,0,"div",8),s.Nc(20,me,1,0,"app-sxc-insights",5),s.Vb(),s.Vb()),2&e&&(s.Bb(3),s.Rc("Manage Apps in Zone ",t.zoneId,""),s.Bb(5),s.qc("selectedIndex",t.tabIndex))},directives:[r.i,l.b,p.a,u.a,a.h,b.c,b.a,b.d,b.b,k,M,Y,se],styles:[""]}),e}()}}]);
//# sourceMappingURL=apps-management-nav-component.5c33fa3e2f30b5b3df73.js.map