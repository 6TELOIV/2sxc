(window.webpackJsonp=window.webpackJsonp||[]).push([[25],{YLeX:function(e,t,n){"use strict";n.r(t);var o=n("D57K"),i=function(e){function t(){return null!==e&&e.apply(this,arguments)||this}return Object(o.d)(t,e),t}(function(){return function(){}}()),a=n("o9tz"),c=n("Y2qJ"),s=n("1C3z"),r=n("BLjT"),p=n("5/c3"),u=n("S36y"),b=n("2pW/"),l=n("8AiQ"),d=n("ZSGP"),f=n("hOvr"),m=n("TDrE"),h=n("FONI"),y=n("LuBX"),g=n("OeRG"),S=n("9HSk"),v=n("Qc/f"),T=n("Uk43"),D=n("Zfm5"),C=n("r4gC");function V(e,t){1&e&&(s.Wb(0,"app-field-hint",24),s.Pc(1,"This field is required"),s.Vb()),2&e&&s.qc("isError",!0)}function W(e,t){if(1&e&&(s.Wb(0,"app-field-hint",24),s.Pc(1),s.Vb()),2&e){var n=s.ic(3);s.qc("isError",!0),s.Bb(1),s.Qc(n.contentTypeNameError)}}function P(e,t){if(1&e&&(s.Ub(0),s.Nc(1,V,2,1,"app-field-hint",23),s.Nc(2,W,2,2,"app-field-hint",23),s.Tb()),2&e){s.ic();var n=s.Cc(7);s.Bb(1),s.qc("ngIf",n.errors.required),s.Bb(1),s.qc("ngIf",n.errors.pattern)}}function k(e,t){if(1&e&&(s.Wb(0,"mat-option",25),s.Pc(1),s.Vb()),2&e){var n=t.$implicit;s.qc("value",n.value),s.Bb(1),s.Rc("",n.name," ")}}function N(e,t){if(1&e){var n=s.Xb();s.Wb(0,"mat-icon",26),s.ec("click",(function(e){return s.Ec(n),s.ic(2).unlockScope(e)})),s.Pc(1,"lock"),s.Vb()}}function I(e,t){if(1&e){var n=s.Xb();s.Wb(0,"mat-icon",26),s.ec("click",(function(e){return s.Ec(n),s.ic(2).unlockScope(e)})),s.Pc(1,"lock_open"),s.Vb()}}function O(e,t){if(1&e&&(s.Wb(0,"div",5),s.Wb(1,"h3"),s.Pc(2,"Shared Content Type (Ghost)"),s.Vb(),s.Wb(3,"p"),s.Pc(4,"Note: this can't be edited in the UI, for now if you really know what you're doing, do it in the DB"),s.Vb(),s.Wb(5,"p"),s.Pc(6),s.Vb(),s.Vb()),2&e){var n=s.ic(2);s.Bb(6),s.Rc("Uses Type Definition of: ",n.contentType.SharedDefId,"")}}function B(e,t){if(1&e){var n=s.Xb();s.Wb(0,"form",3,4),s.ec("ngSubmit",(function(){return s.Ec(n),s.ic().onSubmit()})),s.Wb(2,"div",5),s.Wb(3,"mat-form-field",6),s.Wb(4,"mat-label"),s.Pc(5,"Name"),s.Vb(),s.Wb(6,"input",7,8),s.ec("ngModelChange",(function(e){return s.Ec(n),s.ic().contentType.Name=e})),s.Vb(),s.Vb(),s.Nc(8,P,3,2,"ng-container",9),s.Vb(),s.Wb(9,"mat-accordion"),s.Wb(10,"mat-expansion-panel",10),s.Wb(11,"mat-expansion-panel-header"),s.Wb(12,"mat-panel-title"),s.Pc(13,"Advanced"),s.Vb(),s.Rb(14,"mat-panel-description"),s.Vb(),s.Wb(15,"div",5),s.Wb(16,"mat-form-field",6),s.Wb(17,"mat-label"),s.Pc(18,"Description"),s.Vb(),s.Wb(19,"input",11),s.ec("ngModelChange",(function(e){return s.Ec(n),s.ic().contentType.Description=e})),s.Vb(),s.Vb(),s.Vb(),s.Wb(20,"div",5),s.Wb(21,"mat-form-field",6),s.Wb(22,"mat-label"),s.Pc(23,"Scope"),s.Vb(),s.Wb(24,"mat-select",12),s.ec("selectionChange",(function(e){return s.Ec(n),s.ic().changeScope(e)})),s.Nc(25,k,2,2,"mat-option",13),s.Wb(26,"mat-option",14),s.Pc(27,"Other..."),s.Vb(),s.Vb(),s.Wb(28,"button",15),s.Nc(29,N,2,0,"mat-icon",16),s.Nc(30,I,2,0,"mat-icon",16),s.Vb(),s.Vb(),s.Wb(31,"app-field-hint"),s.Pc(32," The scope should almost never be changed - "),s.Wb(33,"a",17),s.Pc(34,"see help"),s.Vb(),s.Vb(),s.Vb(),s.Wb(35,"div",5),s.Wb(36,"mat-form-field",6),s.Wb(37,"mat-label"),s.Pc(38,"Static Name"),s.Vb(),s.Rb(39,"input",18),s.Vb(),s.Vb(),s.Nc(40,O,7,1,"div",19),s.Vb(),s.Vb(),s.Wb(41,"div",20),s.Wb(42,"button",21),s.ec("click",(function(){return s.Ec(n),s.ic().closeDialog()})),s.Pc(43,"Cancel"),s.Vb(),s.Wb(44,"button",22),s.Pc(45,"Save"),s.Vb(),s.Vb(),s.Vb()}if(2&e){var o=s.Cc(1),i=s.Cc(7),a=s.ic();s.Bb(6),s.qc("pattern",a.contentTypeNamePattern)("ngModel",a.contentType.Name),s.Bb(2),s.qc("ngIf",i.touched&&i.errors),s.Bb(1),s.qc("@.disabled",a.disableAnimation),s.Bb(10),s.qc("ngModel",a.contentType.Description),s.Bb(5),s.qc("ngModel",a.contentType.Scope)("disabled",a.lockScope),s.Bb(1),s.qc("ngForOf",a.scopeOptions),s.Bb(3),s.qc("matTooltip",a.lockScope?"Unlock":"Lock"),s.Bb(1),s.qc("ngIf",a.lockScope),s.Bb(1),s.qc("ngIf",!a.lockScope),s.Bb(9),s.qc("ngModel",a.contentType.StaticName),s.Bb(1),s.qc("ngIf",a.contentType.SharedDefId),s.Bb(4),s.qc("disabled",!o.form.valid)}}n.d(t,"EditContentTypeComponent",(function(){return q}));var q=function(){function e(e,t,n,o){var i=this;this.dialogRef=e,this.route=t,this.contentTypesService=n,this.snackBar=o,this.lockScope=!0,this.contentTypeNamePattern=c.b,this.contentTypeNameError=c.a,this.disableAnimation=!0,this.scope=this.route.snapshot.paramMap.get("scope"),this.contentTypesService.getScopes().subscribe((function(e){i.scopeOptions=e})),this.id=parseInt(this.route.snapshot.paramMap.get("id"),10)}return e.prototype.ngAfterViewInit=function(){var e=this;setTimeout((function(){return e.disableAnimation=!1}))},e.prototype.ngOnInit=function(){this.id?this.fetchContentType():this.contentType=Object(o.a)(Object(o.a)({},new i),{StaticName:"",Name:"",Description:"",Scope:this.scope,ChangeStaticName:!1,NewStaticName:""})},e.prototype.changeScope=function(e){var t=e.value;"Other"===t&&((t=prompt("This is an advanced feature to show content-types of another scope. Don't use this if you don't know what you're doing, as content-types of other scopes are usually hidden for a good reason."))?this.scopeOptions.find((function(e){return e.value===t}))||this.scopeOptions.push({name:t,value:t}):t=a.a.scopes.default.value),this.contentType.Scope=t},e.prototype.unlockScope=function(e){e.stopPropagation(),this.lockScope=!this.lockScope,this.lockScope&&(this.contentType.Scope=this.scope)},e.prototype.onSubmit=function(){var e=this;this.snackBar.open("Saving..."),this.contentTypesService.save(this.contentType).subscribe((function(t){e.snackBar.open("Saved",null,{duration:2e3}),e.closeDialog()}))},e.prototype.closeDialog=function(){this.dialogRef.close()},e.prototype.fetchContentType=function(){var e=this;this.contentTypesService.retrieveContentTypes(this.scope).subscribe((function(t){var n=t.find((function(t){return t.Id===e.id}));e.contentType=Object(o.a)(Object(o.a)({},n),{ChangeStaticName:!1,NewStaticName:n.StaticName})}))},e.\u0275fac=function(t){return new(t||e)(s.Qb(r.h),s.Qb(p.a),s.Qb(u.a),s.Qb(b.b))},e.\u0275cmp=s.Kb({type:e,selectors:[["app-edit-content-type"]],decls:4,vars:2,consts:[["mat-dialog-title",""],[1,"dialog-title-box"],[3,"ngSubmit",4,"ngIf"],[3,"ngSubmit"],["ngForm","ngForm"],[1,"edit-input"],["appearance","standard","color","accent"],["matInput","","type","text","name","Name","required","",3,"pattern","ngModel","ngModelChange"],["name","ngModel"],[4,"ngIf"],["expanded","false"],["matInput","","type","text","name","Description",3,"ngModel","ngModelChange"],["name","Scope",3,"ngModel","disabled","selectionChange"],[3,"value",4,"ngFor","ngForOf"],["value","Other"],["mat-icon-button","","type","button","matSuffix","",3,"matTooltip"],[3,"click",4,"ngIf"],["href","http://2sxc.org/help?tag=scope","target","_blank","appClickStopPropagation",""],["matInput","","type","text","name","StaticName","disabled","",3,"ngModel"],["class","edit-input",4,"ngIf"],[1,"dialog-actions-box"],["mat-raised-button","","type","button",3,"click"],["mat-raised-button","","type","submit","color","accent",3,"disabled"],[3,"isError",4,"ngIf"],[3,"isError"],[3,"value"],[3,"click"]],template:function(e,t){1&e&&(s.Wb(0,"div",0),s.Wb(1,"div",1),s.Pc(2),s.Vb(),s.Vb(),s.Nc(3,B,46,14,"form",2)),2&e&&(s.Bb(2),s.Qc(t.id?"Edit Content Type":"New Content Type"),s.Bb(1),s.qc("ngIf",t.contentType))},directives:[r.i,l.t,d.G,d.s,d.t,f.c,f.g,m.b,d.c,d.B,d.x,d.r,d.u,h.a,h.c,h.e,h.f,h.d,y.a,l.s,g.l,S.b,f.j,v.a,T.a,D.a,C.a],styles:[".edit-input[_ngcontent-%COMP%]{padding-bottom:8px}.mat-hint[_ngcontent-%COMP%]{font-size:12px}"]}),e}()},o9tz:function(e,t,n){"use strict";n.d(t,"a",(function(){return o}));var o={metadata:{attribute:{type:2,target:"EAV Field Properties"},app:{type:3,target:"App"},entity:{type:4,target:"Entity"},contentType:{type:5,target:"ContentType"},zone:{type:6,target:"Zone"},cmsObject:{type:10,target:"CmsObject"}},keyTypes:{guid:"guid",string:"string",number:"number"},scopes:{default:{name:"Default",value:"2SexyContent"},app:{name:"App",value:"2SexyContent-App"},cmsSystem:{name:"CMS System",value:"2SexyContent-System"},system:{name:"System",value:"System"}},contentTypes:{template:"2SexyContent-Template",permissions:"PermissionConfiguration",query:"DataPipeline",contentType:"ContentType",settings:"App-Settings",resources:"App-Resources"},pipelineDesigner:{outDataSource:{className:"SexyContentTemplate",in:["ListContent","Default"],name:"2sxc Target (View or API)",description:"The template/script which will show this data",visualDesignerData:{Top:20,Left:200,Width:700}},defaultPipeline:{dataSources:[{entityGuid:"unsaved1",partAssemblyAndType:"ToSic.Eav.DataSources.Caches.ICache, ToSic.Eav.DataSources",visualDesignerData:{Top:440,Left:440}},{entityGuid:"unsaved2",partAssemblyAndType:"ToSic.Eav.DataSources.PublishingFilter, ToSic.Eav.DataSources",visualDesignerData:{Top:300,Left:440}},{entityGuid:"unsaved3",partAssemblyAndType:"ToSic.SexyContent.DataSources.ModuleDataSource, ToSic.SexyContent",visualDesignerData:{Top:170,Left:440}}],streamWiring:[{From:"unsaved1",Out:"Default",To:"unsaved2",In:"Default"},{From:"unsaved1",Out:"Drafts",To:"unsaved2",In:"Drafts"},{From:"unsaved1",Out:"Published",To:"unsaved2",In:"Published"},{From:"unsaved2",Out:"Default",To:"unsaved3",In:"Default"},{From:"unsaved3",Out:"ListContent",To:"Out",In:"ListContent"},{From:"unsaved3",Out:"Default",To:"Out",In:"Default"}]},testParameters:"[Demo:Demo]=true"}}}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.01.00/ng-edit/edit-content-type-component.ca30f8b95127b470bff7.js.map