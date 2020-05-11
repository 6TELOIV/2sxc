(window.webpackJsonp=window.webpackJsonp||[]).push([[23],{o9tz:function(e,t,i){"use strict";i.d(t,"a",(function(){return n}));var n={metadata:{attribute:{type:2,target:"EAV Field Properties"},app:{type:3,target:"App"},entity:{type:4,target:"Entity"},contentType:{type:5,target:"ContentType"},zone:{type:6,target:"Zone"},cmsObject:{type:10,target:"CmsObject"}},keyTypes:{guid:"guid",string:"string",number:"number"},scopes:{default:{name:"Default",value:"2SexyContent"},app:{name:"App",value:"2SexyContent-App"},cmsSystem:{name:"CMS System",value:"2SexyContent-System"},system:{name:"System",value:"System"}},contentTypes:{template:"2SexyContent-Template",permissions:"PermissionConfiguration",query:"DataPipeline",contentType:"ContentType",settings:"App-Settings",resources:"App-Resources"},pipelineDesigner:{outDataSource:{className:"SexyContentTemplate",in:["ListContent","Default"],name:"2sxc Target (View or API)",description:"The template/script which will show this data",visualDesignerData:{Top:20,Left:200,Width:700}},defaultPipeline:{dataSources:[{entityGuid:"unsaved1",partAssemblyAndType:"ToSic.Eav.DataSources.Caches.ICache, ToSic.Eav.DataSources",visualDesignerData:{Top:440,Left:440}},{entityGuid:"unsaved2",partAssemblyAndType:"ToSic.Eav.DataSources.PublishingFilter, ToSic.Eav.DataSources",visualDesignerData:{Top:300,Left:440}},{entityGuid:"unsaved3",partAssemblyAndType:"ToSic.SexyContent.DataSources.ModuleDataSource, ToSic.SexyContent",visualDesignerData:{Top:170,Left:440}}],streamWiring:[{From:"unsaved1",Out:"Default",To:"unsaved2",In:"Default"},{From:"unsaved1",Out:"Drafts",To:"unsaved2",In:"Drafts"},{From:"unsaved1",Out:"Published",To:"unsaved2",In:"Published"},{From:"unsaved2",Out:"Default",To:"unsaved3",In:"Default"},{From:"unsaved3",Out:"ListContent",To:"Out",In:"ListContent"},{From:"unsaved3",Out:"Default",To:"Out",In:"Default"}]},testParameters:"[Demo:Demo]=true"}}},u8xq:function(e,t,i){"use strict";i.r(t);var n=i("D57K"),o=i("5/c3"),a=i("LR82"),s=i("z5yO"),r=i("KLQV"),l=i("o9tz"),c=i("Y2qJ"),p=i("1C3z"),u=i("OeRG"),d=i("Qc/f"),h=i("r4gC"),f=function(){function e(){}return e.prototype.agInit=function(e){this.icon=e.value?"star":"star_border"},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=p.Kb({type:e,selectors:[["app-content-type-fields-title"]],decls:4,vars:1,consts:[[1,"actions-component"],["matRipple","","matTooltip","Use as title field",1,"like-button","highlight"]],template:function(e,t){1&e&&(p.Wb(0,"div",0),p.Wb(1,"div",1),p.Wb(2,"mat-icon"),p.Pc(3),p.Vb(),p.Vb(),p.Vb()),2&e&&(p.Bb(3),p.Qc(t.icon))},directives:[u.q,d.a,h.a],styles:[""]}),e}(),m=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=p.Kb({type:e,selectors:[["app-content-type-fields-input-type"]],decls:6,vars:1,consts:[["matRipple","",1,"input-component","highlight"],[1,"text"],[1,"like-button"]],template:function(e,t){1&e&&(p.Wb(0,"div",0),p.Wb(1,"div",1),p.Pc(2),p.Vb(),p.Wb(3,"div",2),p.Wb(4,"mat-icon"),p.Pc(5,"arrow_drop_down"),p.Vb(),p.Vb(),p.Vb()),2&e&&(p.Bb(2),p.Qc(t.value))},directives:[u.q,h.a],styles:[""]}),e}(),y=i("nsG0"),b=i("DGvA"),g=i("8AiQ");function v(e,t){if(1&e){var i=p.Xb();p.Wb(0,"div",5),p.ec("click",(function(){return p.Ec(i),p.ic().delete()})),p.Wb(1,"mat-icon"),p.Pc(2,"delete"),p.Vb(),p.Vb()}}function T(e,t){1&e&&(p.Wb(0,"div",6),p.Wb(1,"mat-icon"),p.Pc(2,"delete"),p.Vb(),p.Vb())}function C(e,t){if(1&e){var i=p.Xb();p.Wb(0,"div",7),p.ec("click",(function(){return p.Ec(i),p.ic().openPermissions()})),p.Wb(1,"mat-icon"),p.Pc(2,"person"),p.Vb(),p.Vb()}}var D=function(){function e(){}return e.prototype.agInit=function(e){this.params=e,this.field=e.data,this.showPermissions=this.field.InputType===y.a.StringWysiwyg||this.field.Type===b.a.Hyperlink},e.prototype.refresh=function(e){return!0},e.prototype.rename=function(){this.params.onRename(this.field)},e.prototype.delete=function(){this.params.onDelete(this.field)},e.prototype.openPermissions=function(){this.params.onOpenPermissions(this.field)},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=p.Kb({type:e,selectors:[["app-content-type-fields-actions"]],decls:7,vars:3,consts:[[1,"actions-component"],["matRipple","","matTooltip","Rename",1,"like-button","highlight",3,"click"],["class","like-button highlight","matRipple","","matTooltip","Delete",3,"click",4,"ngIf"],["class","like-button disabled",4,"ngIf"],["class","like-button highlight","matRipple","","matTooltip","Permissions",3,"click",4,"ngIf"],["matRipple","","matTooltip","Delete",1,"like-button","highlight",3,"click"],[1,"like-button","disabled"],["matRipple","","matTooltip","Permissions",1,"like-button","highlight",3,"click"]],template:function(e,t){1&e&&(p.Wb(0,"div",0),p.Wb(1,"div",1),p.ec("click",(function(){return t.rename()})),p.Wb(2,"mat-icon"),p.Pc(3,"settings"),p.Vb(),p.Vb(),p.Nc(4,v,3,0,"div",2),p.Nc(5,T,3,0,"div",3),p.Nc(6,C,3,0,"div",4),p.Vb()),2&e&&(p.Bb(4),p.qc("ngIf",!t.field.IsTitle),p.Bb(1),p.qc("ngIf",t.field.IsTitle),p.Bb(1),p.qc("ngIf",t.showPermissions))},directives:[u.q,d.a,h.a,g.t],styles:[""]}),e}(),S=i("DOM6"),w=function(){function e(){}return e.prototype.agInit=function(e){this.value=e.value,this.icon=Object(S.a)(this.value)},e.prototype.refresh=function(e){return!0},e.\u0275fac=function(t){return new(t||e)},e.\u0275cmp=p.Kb({type:e,selectors:[["app-content-type-fields-type"]],decls:3,vars:2,consts:[[1,"icon-container",3,"matTooltip"]],template:function(e,t){1&e&&(p.Wb(0,"div",0),p.Wb(1,"mat-icon"),p.Pc(2),p.Vb(),p.Vb()),2&e&&(p.qc("matTooltip",t.value),p.Bb(2),p.Qc(t.icon))},directives:[d.a,h.a],styles:[""]}),e}(),I=i("BLjT"),F=i("S36y"),k=i("/Foi"),R=i("2pW/"),O=i("9HSk"),A=i("G6Ml");i.d(t,"ContentTypeFieldsComponent",(function(){return P}));var P=function(){function e(e,t,i,n,o,s){this.dialogRef=e,this.route=t,this.router=i,this.contentTypesService=n,this.contentTypesFieldsService=o,this.snackBar=s,this.enableTextSelection=!0,this.columnDefs=[{rowDrag:!0,width:18,cellClass:"no-padding"},{headerName:"Title",field:"IsTitle",width:42,cellClass:"secondary-action no-padding no-outline",cellRenderer:"contentTypeFieldsTitleComponent",onCellClicked:this.setTitle.bind(this)},{headerName:"Name",field:"StaticName",flex:2,minWidth:250,cellClass:"primary-action highlight",sortable:!0,filter:"agTextColumnFilter",onCellClicked:this.editFieldMetadata.bind(this)},{headerName:"Type",field:"Type",width:70,headerClass:"dense",cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter",cellRenderer:"contentTypeFieldsTypeComponent"},{headerName:"Input",field:"InputType",width:160,cellClass:"secondary-action no-padding",sortable:!0,filter:"agTextColumnFilter",cellRenderer:"contentTypeFieldsInputTypeComponent",onCellClicked:this.changeInputType.bind(this),valueGetter:this.inputTypeValueGetter},{width:120,cellClass:"secondary-action no-padding",cellRenderer:"contentTypeFieldsActionsComponent",cellRendererParams:{onRename:this.rename.bind(this),onDelete:this.delete.bind(this),onOpenPermissions:this.openPermissions.bind(this)}},{headerName:"Label",field:"Metadata.All.Name",flex:2,minWidth:250,cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter"},{headerName:"Notes",field:"Metadata.All.Notes",flex:2,minWidth:250,cellClass:"no-outline",sortable:!0,filter:"agTextColumnFilter"}],this.frameworkComponents={contentTypeFieldsTitleComponent:f,contentTypeFieldsTypeComponent:w,contentTypeFieldsInputTypeComponent:m,contentTypeFieldsActionsComponent:D},this.modules=r.a,this.gridOptions={getRowClass:function(e){return e.data.InputType===y.a.EmptyDefault?"group-row":""}},this.sortApplied=!1,this.filterApplied=!1,this.rowDragSuppressed=!1,this.subscription=new a.a,this.hasChild=!!this.route.snapshot.firstChild,this.contentTypeStaticName=this.route.snapshot.paramMap.get("contentTypeStaticName")}return e.prototype.ngOnInit=function(){return Object(n.b)(this,void 0,void 0,(function(){var e;return Object(n.e)(this,(function(t){switch(t.label){case 0:return e=this,[4,this.contentTypesService.retrieveContentType(this.contentTypeStaticName).toPromise()];case 1:return e.contentType=t.sent(),[4,this.fetchFields()];case 2:return t.sent(),this.refreshOnChildClosed(),[2]}}))}))},e.prototype.ngOnDestroy=function(){this.subscription.unsubscribe(),this.subscription=null},e.prototype.onGridReady=function(e){this.gridApi=e.api},e.prototype.onRowDragEnter=function(e){this.enableTextSelection=!1},e.prototype.onRowDragEnd=function(e){var t=this;this.enableTextSelection=!0;var i=this.fields.map((function(e){return e.Id}));this.contentTypesFieldsService.reOrder(i,this.contentType).subscribe((function(e){t.fetchFields()}))},e.prototype.onRowDragMove=function(e){var t=e.node,i=e.overNode;if(t!==i){var n=i.data,o=this.fields.indexOf(t.data),a=this.fields.indexOf(n),s=this.fields.slice();this.moveInArray(s,o,a),this.fields=s,this.gridApi.setRowData(s),this.gridApi.clearFocusedCell()}},e.prototype.moveInArray=function(e,t,i){var n=e[t];e.splice(t,1),e.splice(i,0,n)},e.prototype.onSortChanged=function(e){var t=this.gridApi.getSortModel();this.sortApplied=t.length>0,this.suppressRowDrag()},e.prototype.onFilterChanged=function(e){var t=this.gridApi.getFilterModel(),i=Object.keys(t);this.filterApplied=i.length>0,this.suppressRowDrag()},e.prototype.suppressRowDrag=function(){var e=this.sortApplied||this.filterApplied;e&&!this.rowDragSuppressed?(this.rowDragSuppressed=!0,this.gridApi.setSuppressRowDrag(!0)):!e&&this.rowDragSuppressed&&(this.rowDragSuppressed=!1,this.gridApi.setSuppressRowDrag(!1))},e.prototype.closeDialog=function(){this.dialogRef.close()},e.prototype.add=function(){this.router.navigate(["add/"+this.contentTypeStaticName],{relativeTo:this.route})},e.prototype.inputTypeValueGetter=function(e){var t=e.data;return t.InputType.substring(t.InputType.indexOf("-")+1)},e.prototype.fetchFields=function(){return Object(n.b)(this,void 0,void 0,(function(){var e;return Object(n.e)(this,(function(t){switch(t.label){case 0:return e=this,[4,this.contentTypesFieldsService.getFields(this.contentType).toPromise()];case 1:return e.fields=t.sent(),[2]}}))}))},e.prototype.editFieldMetadata=function(e){var t=e.data,i={items:[this.createItemDefinition(t,"All"),this.createItemDefinition(t,t.Type),this.createItemDefinition(t,t.InputType)]};this.router.navigate(["edit/"+JSON.stringify(i)],{relativeTo:this.route})},e.prototype.createItemDefinition=function(e,t){return void 0!==e.Metadata[t]?{EntityId:e.Metadata[t].Id}:{ContentTypeName:"@"+t,For:{Target:l.a.metadata.attribute.target,Number:e.Id},Prefill:{Name:e.StaticName}}},e.prototype.setTitle=function(e){var t=this,i=e.data;this.snackBar.open("Setting title..."),this.contentTypesFieldsService.setTitle(i,this.contentType).subscribe((function(){t.snackBar.open("Title set",null,{duration:2e3}),t.fetchFields()}))},e.prototype.changeInputType=function(e){this.router.navigate(["update/"+this.contentTypeStaticName+"/"+e.data.Id],{relativeTo:this.route})},e.prototype.rename=function(e){var t=this,i=prompt("What new name would you like for '"+e.StaticName+"' ("+e.Id+")?",e.StaticName);if(null!==i&&(i=i.trim().replace(/\s\s+/g," "))!==e.StaticName){for(;!i.match(c.b);){if(null===(i=prompt("What new name would you like for '"+e.StaticName+"' ("+e.Id+")?\n"+c.a,i)))return;if((i=i.trim().replace(/\s\s+/g," "))===e.StaticName)return}this.snackBar.open("Saving..."),this.contentTypesFieldsService.rename(e,this.contentType,i).subscribe((function(){t.snackBar.open("Saved",null,{duration:2e3}),t.fetchFields()}))}},e.prototype.delete=function(e){var t=this;confirm("Are you sure you want to delete '"+e.StaticName+"' ("+e.Id+")?")&&(this.snackBar.open("Deleting..."),this.contentTypesFieldsService.delete(e,this.contentType).subscribe((function(e){t.snackBar.open("Deleted",null,{duration:2e3}),t.fetchFields()})))},e.prototype.openPermissions=function(e){this.router.navigate(["permissions/"+l.a.metadata.attribute.type+"/"+l.a.keyTypes.number+"/"+e.Id],{relativeTo:this.route})},e.prototype.refreshOnChildClosed=function(){var e=this;this.subscription.add(this.router.events.pipe(Object(s.a)((function(e){return e instanceof o.b}))).subscribe((function(t){var i=e.hasChild;e.hasChild=!!e.route.snapshot.firstChild,!e.hasChild&&i&&e.fetchFields()})))},e.\u0275fac=function(t){return new(t||e)(p.Qb(I.h),p.Qb(o.a),p.Qb(o.c),p.Qb(F.a),p.Qb(k.a),p.Qb(R.b))},e.\u0275cmp=p.Kb({type:e,selectors:[["app-content-type-fields"]],decls:13,vars:12,consts:[["mat-dialog-title",""],[1,"dialog-title-box"],["mat-icon-button","","matTooltip","Close dialog",3,"click"],[1,"grid-wrapper"],[1,"ag-theme-material",3,"rowData","columnDefs","modules","frameworkComponents","enableCellTextSelection","suppressScrollOnNewData","animateRows","accentedSort","valueCache","gridOptions","headerHeight","rowDragEnter","rowDragEnd","rowDragMove","gridReady","sortChanged","filterChanged"],["mat-fab","","mat-elevation-z24","","matTooltip","Add fields",1,"grid-fab",3,"click"]],template:function(e,t){1&e&&(p.Wb(0,"div",0),p.Wb(1,"div",1),p.Wb(2,"div"),p.Pc(3,"Content Type Fields"),p.Vb(),p.Wb(4,"button",2),p.ec("click",(function(){return t.closeDialog()})),p.Wb(5,"mat-icon"),p.Pc(6,"close"),p.Vb(),p.Vb(),p.Vb(),p.Vb(),p.Rb(7,"router-outlet"),p.Wb(8,"div",3),p.Wb(9,"ag-grid-angular",4),p.ec("rowDragEnter",(function(e){return t.onRowDragEnter(e)}))("rowDragEnd",(function(e){return t.onRowDragEnd(e)}))("rowDragMove",(function(e){return t.onRowDragMove(e)}))("gridReady",(function(e){return t.onGridReady(e)}))("sortChanged",(function(e){return t.onSortChanged(e)}))("filterChanged",(function(e){return t.onFilterChanged(e)})),p.Vb(),p.Wb(10,"button",5),p.ec("click",(function(){return t.add()})),p.Wb(11,"mat-icon"),p.Pc(12,"add"),p.Vb(),p.Vb(),p.Vb()),2&e&&(p.Bb(9),p.qc("rowData",t.fields)("columnDefs",t.columnDefs)("modules",t.modules)("frameworkComponents",t.frameworkComponents)("enableCellTextSelection",t.enableTextSelection)("suppressScrollOnNewData",!0)("animateRows",!0)("animateRows",!0)("accentedSort",!0)("valueCache",!0)("gridOptions",t.gridOptions)("headerHeight",32))},directives:[I.i,O.b,d.a,h.a,o.h,A.a],styles:["ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 176px)}@media (max-width:600px){ag-grid-angular.ag-theme-material[_ngcontent-%COMP%]{height:calc(100vh - 146px)}}"]}),e}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.01.00/ng-edit/content-type-fields-component.5f6b20c5d3a319e27391.js.map