(window.webpackJsonp=window.webpackJsonp||[]).push([[26],{"87pQ":function(t,e,n){"use strict";n.r(e);var i=n("D57K"),o=n("DOM6"),c={Boolean:{label:"Boolean (yes/no)",description:"Yes/no or true/false values"},Custom:{label:"Custom - ui-tools or custom types",description:"Use for things like gps-pickers (which writes into multiple fields) or for custom-data which serializes something exotic into the db like an array, a custom json or anything"},DateTime:{label:"Date and/or time",description:"For date, time or combined values"},Empty:{label:"Empty - for form-titles etc.",description:"Use to structure your form"},Entity:{label:"Entity (other content-items)",description:"One or more other content-items"},Hyperlink:{label:"Link / file reference",description:"Hyperlink or reference to a picture / file"},Number:{label:"Number",description:"Any kind of number"},String:{label:"Text / string",description:"Any kind of text"}},r=n("Y2qJ"),a=n("nsG0"),s=n("1C3z"),p=n("BLjT"),l=n("5/c3"),d=n("S36y"),u=n("/Foi"),b=n("2pW/"),f=n("8AiQ"),h=n("ZSGP"),y=n("9HSk"),m=n("hOvr"),g=n("TDrE"),T=n("LuBX"),v=n("r4gC"),S=n("Uk43"),I=n("OeRG");function O(t,e){1&t&&(s.Ub(0),s.Pc(1,"Fields"),s.Tb())}function C(t,e){1&t&&(s.Ub(0),s.Pc(1,"Edit Field"),s.Tb())}function W(t,e){1&t&&(s.Ub(0),s.Pc(1,"Add Fields"),s.Tb())}function B(t,e){if(1&t&&(s.Wb(0,"app-field-hint",21),s.Pc(1),s.Vb()),2&t){var n=s.ic(4);s.qc("isError",!0),s.Bb(1),s.Qc(n.contentTypeNameError)}}function V(t,e){if(1&t&&(s.Ub(0),s.Nc(1,B,2,2,"app-field-hint",20),s.Tb()),2&t){s.ic();var n=s.Cc(6);s.Bb(1),s.qc("ngIf",n.errors.pattern)}}function w(t,e){if(1&t&&(s.Wb(0,"mat-option",22),s.Wb(1,"mat-icon"),s.Pc(2),s.Vb(),s.Wb(3,"span"),s.Pc(4),s.Vb(),s.Vb()),2&t){var n=e.$implicit;s.qc("value",n.name),s.Bb(2),s.Qc(n.icon),s.Bb(2),s.Qc(n.label)}}function P(t,e){if(1&t&&(s.Wb(0,"mat-option",22),s.Pc(1),s.Vb()),2&t){var n=e.$implicit;s.qc("value",n.inputType),s.Bb(1),s.Rc(" ",n.label,"")}}function M(t,e){if(1&t){var n=s.Xb();s.Wb(0,"div",10),s.Wb(1,"div",11),s.Wb(2,"mat-form-field",12),s.Wb(3,"mat-label"),s.Pc(4,"Name"),s.Vb(),s.Wb(5,"input",13,14),s.ec("ngModelChange",(function(t){return s.Ec(n),e.$implicit.StaticName=t})),s.Vb(),s.Vb(),s.Nc(7,V,2,1,"ng-container",15),s.Vb(),s.Wb(8,"div",11),s.Wb(9,"mat-form-field",12),s.Wb(10,"mat-label"),s.Pc(11,"Data Type"),s.Vb(),s.Wb(12,"mat-select",16),s.ec("selectionChange",(function(){s.Ec(n);var t=e.index,i=s.ic(2);return i.resetInputType(t),i.calculateInputTypeOptions(t),i.calculateHints(t)}))("ngModelChange",(function(t){return s.Ec(n),e.$implicit.Type=t})),s.Wb(13,"mat-select-trigger"),s.Wb(14,"mat-icon",17),s.Pc(15),s.Vb(),s.Wb(16,"span"),s.Pc(17),s.Vb(),s.Vb(),s.Nc(18,w,5,3,"mat-option",18),s.Vb(),s.Vb(),s.Wb(19,"app-field-hint"),s.Pc(20),s.Vb(),s.Vb(),s.Wb(21,"div",11),s.Wb(22,"mat-form-field",12),s.Wb(23,"mat-label"),s.Pc(24,"Input Type"),s.Vb(),s.Wb(25,"mat-select",19),s.ec("selectionChange",(function(){s.Ec(n);var t=e.index;return s.ic(2).calculateHints(t)}))("ngModelChange",(function(t){return s.Ec(n),e.$implicit.InputType=t})),s.Nc(26,P,2,2,"mat-option",18),s.Vb(),s.Vb(),s.Wb(27,"app-field-hint"),s.Pc(28),s.Vb(),s.Vb(),s.Vb()}if(2&t){var i=e.$implicit,o=e.index,c=s.Cc(6),r=s.ic(2);s.Bb(5),s.qc("pattern",r.contentTypeNamePattern)("ngModel",i.StaticName)("name","StaticName"+o)("disabled",r.editMode),s.Bb(2),s.qc("ngIf",c.touched&&c.errors),s.Bb(5),s.qc("ngModel",i.Type)("name","Type"+o)("disabled",r.editMode),s.Bb(3),s.Qc(r.findIcon(i.Type)),s.Bb(2),s.Qc(i.Type),s.Bb(1),s.qc("ngForOf",r.dataTypes),s.Bb(2),s.Qc(r.dataTypeHints[o]),s.Bb(5),s.qc("ngModel",i.InputType)("name","InputType"+o),s.Bb(1),s.qc("ngForOf",r.filteredInputTypeOptions[o]),s.Bb(2),s.Qc(r.inputTypeHints[o])}}function N(t,e){if(1&t){var n=s.Xb();s.Wb(0,"form",4,5),s.ec("ngSubmit",(function(){return s.Ec(n),s.ic().onSubmit()})),s.Nc(2,M,29,16,"div",6),s.Wb(3,"div",7),s.Wb(4,"button",8),s.ec("click",(function(){return s.Ec(n),s.ic().closeDialog()})),s.Pc(5,"Cancel"),s.Vb(),s.Wb(6,"button",9),s.Pc(7,"Save"),s.Vb(),s.Vb(),s.Vb()}if(2&t){var i=s.Cc(1),o=s.ic();s.Bb(2),s.qc("ngForOf",o.fields),s.Bb(4),s.qc("disabled",!i.form.valid)}}n.d(e,"EditContentTypeFieldsComponent",(function(){return F}));var F=function(){function t(t,e,n,i,o){this.dialogRef=t,this.route=e,this.contentTypesService=n,this.contentTypesFieldsService=i,this.snackBar=o,this.filteredInputTypeOptions=[],this.dataTypeHints=[],this.inputTypeHints=[],this.contentTypeNamePattern=r.b,this.contentTypeNameError=r.a,this.contentTypeStaticName=this.route.snapshot.paramMap.get("contentTypeStaticName")}return t.prototype.ngOnInit=function(){return Object(i.b)(this,void 0,void 0,(function(){var t,e,n,r,a,s,p;return Object(i.e)(this,(function(l){switch(l.label){case 0:return t=this,[4,this.contentTypesService.retrieveContentType(this.contentTypeStaticName).toPromise()];case 1:return t.contentType=l.sent(),[4,this.contentTypesFieldsService.getFields(this.contentType).toPromise()];case 2:return e=l.sent(),n=this.route.snapshot.paramMap.get("id")?parseInt(this.route.snapshot.paramMap.get("id"),10):null,this.editMode=null!==n,[4,this.contentTypesFieldsService.typeListRetrieve().toPromise()];case 3:return r=l.sent(),this.dataTypes=function(t){var e,n,r=[];try{for(var a=Object(i.i)(t),s=a.next();!s.done;s=a.next()){var p=s.value;r.push({name:p,label:c[p].label,icon:Object(o.a)(p),description:c[p].description})}}catch(l){e={error:l}}finally{try{s&&!s.done&&(n=a.return)&&n.call(a)}finally{if(e)throw e.error}}return r}(r),a=this,[4,this.contentTypesFieldsService.getInputTypesList().toPromise()];case 4:if(a.inputTypeOptions=l.sent(),this.editMode)s=e.find((function(t){return t.Id===n})),this.fields=[s];else for(this.fields=[],p=1;p<=8;p++)this.fields.push({Id:0,Type:"String",InputType:"string-default",StaticName:"",IsTitle:0===e.length,SortOrder:e.length+p});for(p=0;p<this.fields.length;p++)this.calculateInputTypeOptions(p),this.calculateHints(p);return[2]}}))}))},t.prototype.closeDialog=function(){this.dialogRef.close()},t.prototype.resetInputType=function(t){this.fields[t].InputType=this.fields[t].Type.toLowerCase()+a.a.DefaultSuffix},t.prototype.calculateInputTypeOptions=function(t){var e=this;this.filteredInputTypeOptions[t]=this.inputTypeOptions.filter((function(n){return n.dataType===e.fields[t].Type.toLowerCase()}))},t.prototype.calculateHints=function(t){var e=this,n=this.dataTypes.find((function(n){return n.name===e.fields[t].Type})),i=this.inputTypeOptions.find((function(n){return n.inputType===e.fields[t].InputType}));this.dataTypeHints[t]=n?n.description:"",this.inputTypeHints[t]=i?i.description:""},t.prototype.onSubmit=function(){return Object(i.b)(this,void 0,void 0,(function(){var t,e,n,o,c,r;return Object(i.e)(this,(function(a){switch(a.label){case 0:return this.snackBar.open("Saving..."),this.editMode?[4,this.contentTypesFieldsService.updateInputType(this.fields[0].Id,this.fields[0].StaticName,this.fields[0].InputType).toPromise()]:[3,2];case 1:return a.sent(),[3,10];case 2:t=this.fields.filter((function(t){return t.StaticName})),a.label=3;case 3:a.trys.push([3,8,9,10]),e=Object(i.i)(t),n=e.next(),a.label=4;case 4:return n.done?[3,7]:[4,this.contentTypesFieldsService.add(n.value,this.contentType.Id).toPromise()];case 5:a.sent(),a.label=6;case 6:return n=e.next(),[3,4];case 7:return[3,10];case 8:return o=a.sent(),c={error:o},[3,10];case 9:try{n&&!n.done&&(r=e.return)&&r.call(e)}finally{if(c)throw c.error}return[7];case 10:return this.snackBar.open("Saved",null,{duration:2e3}),this.closeDialog(),[2]}}))}))},t.prototype.findIcon=function(t){return Object(o.a)(t)},t.\u0275fac=function(e){return new(e||t)(s.Qb(p.h),s.Qb(l.a),s.Qb(d.a),s.Qb(u.a),s.Qb(b.b))},t.\u0275cmp=s.Kb({type:t,selectors:[["app-edit-content-type-fields"]],decls:6,vars:5,consts:[["mat-dialog-title",""],[1,"dialog-title-box",3,"ngSwitch"],[4,"ngSwitchCase"],[3,"ngSubmit",4,"ngIf"],[3,"ngSubmit"],["ngForm","ngForm"],["class","row-container",4,"ngFor","ngForOf"],[1,"dialog-actions-box"],["mat-raised-button","","type","button",3,"click"],["mat-raised-button","","color","accent","type","submit",3,"disabled"],[1,"row-container"],[1,"edit-input"],["appearance","standard","color","accent"],["matInput","",3,"pattern","ngModel","name","disabled","ngModelChange"],["staticName","ngModel"],[4,"ngIf"],[3,"ngModel","name","disabled","selectionChange","ngModelChange"],[1,"type-icon"],[3,"value",4,"ngFor","ngForOf"],[3,"ngModel","name","selectionChange","ngModelChange"],[3,"isError",4,"ngIf"],[3,"isError"],[3,"value"]],template:function(t,e){1&t&&(s.Wb(0,"div",0),s.Wb(1,"div",1),s.Nc(2,O,2,0,"ng-container",2),s.Nc(3,C,2,0,"ng-container",2),s.Nc(4,W,2,0,"ng-container",2),s.Vb(),s.Vb(),s.Nc(5,N,8,2,"form",3)),2&t&&(s.Bb(1),s.qc("ngSwitch",e.editMode),s.Bb(1),s.qc("ngSwitchCase",void 0),s.Bb(1),s.qc("ngSwitchCase",!0),s.Bb(1),s.qc("ngSwitchCase",!1),s.Bb(1),s.qc("ngIf",e.fields))},directives:[p.i,f.x,f.y,f.t,h.G,h.s,h.t,f.s,y.b,m.c,m.g,g.b,h.c,h.x,h.r,h.u,T.a,T.c,v.a,S.a,I.l],styles:[".row-container[_ngcontent-%COMP%]{display:flex;justify-content:space-between}.edit-input[_ngcontent-%COMP%]{padding-bottom:8px;width:30%}.type-icon[_ngcontent-%COMP%]{width:inherit;height:inherit;margin-right:8px;font-size:inherit;vertical-align:top;line-height:inherit}"]}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.01.00/ng-edit/edit-content-type-fields-component.5bb27a027ce3b6c6fdb0.js.map