(window.webpackJsonp=window.webpackJsonp||[]).push([[20],{pWkR:function(t,i,e){"use strict";e.r(i),e.d(i,"ContentItemImportComponent",(function(){return v}));var n=e("D57K"),c=e("1C3z"),o=e("BLjT"),a=e("55Ui"),b=e("8AiQ"),r=e("9HSk"),s=e("Qc/f"),l=e("nYrE");function u(t,i){1&t&&(c.Wb(0,"span"),c.Pc(1,"Select file"),c.Vb())}function p(t,i){if(1&t&&(c.Wb(0,"span"),c.Pc(1),c.Vb()),2&t){var e=c.ic(2);c.Bb(1),c.Qc(e.importFile.name)}}function d(t,i){if(1&t){var e=c.Xb();c.Wb(0,"div"),c.Wb(1,"div"),c.Wb(2,"button",4),c.ec("click",(function(){return c.Ec(e),c.Cc(6).click()})),c.Nc(3,u,2,0,"span",5),c.Nc(4,p,2,1,"span",5),c.Vb(),c.Wb(5,"input",6,7),c.ec("change",(function(t){return c.Ec(e),c.ic().fileChange(t)})),c.Vb(),c.Vb(),c.Wb(7,"div",8),c.Wb(8,"button",9),c.ec("click",(function(){return c.Ec(e),c.ic().closeDialog()})),c.Pc(9,"Cancel"),c.Vb(),c.Wb(10,"button",10),c.ec("click",(function(){return c.Ec(e),c.ic().importContentItem()})),c.Pc(11,"Import"),c.Vb(),c.Vb(),c.Vb()}if(2&t){var n=c.ic();c.Bb(3),c.qc("ngIf",!n.importFile),c.Bb(1),c.qc("ngIf",n.importFile),c.Bb(6),c.qc("disabled",!n.importFile)}}function f(t,i){1&t&&(c.Wb(0,"div"),c.Rb(1,"mat-spinner",11),c.Wb(2,"div",8),c.Wb(3,"button",12),c.Pc(4,"Cancel"),c.Vb(),c.Wb(5,"button",13),c.Pc(6,"Import"),c.Vb(),c.Vb(),c.Vb())}function m(t,i){if(1&t){var e=c.Xb();c.Wb(0,"div"),c.Wb(1,"p"),c.Pc(2,"Import completed!"),c.Vb(),c.Wb(3,"div",8),c.Wb(4,"button",14),c.ec("click",(function(){return c.Ec(e),c.ic().closeDialog()})),c.Pc(5,"Close"),c.Vb(),c.Vb(),c.Vb()}}var v=function(){function t(t,i){this.dialogRef=t,this.contentItemsService=i,this.viewStates={Default:1,Waiting:2,Imported:3},this.viewState=this.viewStates.Default}return t.prototype.ngOnInit=function(){},t.prototype.importContentItem=function(){return Object(n.b)(this,void 0,void 0,(function(){var t=this;return Object(n.e)(this,(function(i){switch(i.label){case 0:return this.viewState=this.viewStates.Waiting,[4,this.contentItemsService.importItem(this.importFile)];case 1:return i.sent().subscribe({next:function(i){t.viewState=t.viewStates.Imported},error:function(){t.viewState=t.viewStates.Default}}),[2]}}))}))},t.prototype.fileChange=function(t){this.importFile=t.target.files[0]},t.prototype.closeDialog=function(){this.dialogRef.close()},t.\u0275fac=function(i){return new(i||t)(c.Qb(o.h),c.Qb(a.a))},t.\u0275cmp=c.Kb({type:t,selectors:[["app-content-item-import"]],decls:7,vars:4,consts:[["mat-dialog-title",""],[1,"dialog-title-box"],[3,"ngSwitch"],[4,"ngSwitchCase"],["mat-raised-button","","matTooltip","Open file browser",3,"click"],[4,"ngIf"],["type","file",1,"hide",3,"change"],["fileInput",""],[1,"dialog-actions-box"],["mat-raised-button","",3,"click"],["mat-raised-button","","color","accent",3,"disabled","click"],["mode","indeterminate","diameter","20","color","accent"],["mat-raised-button","","disabled",""],["mat-raised-button","","color","accent","disabled",""],["mat-raised-button","","color","accent",3,"click"]],template:function(t,i){1&t&&(c.Wb(0,"div",0),c.Wb(1,"div",1),c.Pc(2,"Import a single JSON Item"),c.Vb(),c.Vb(),c.Wb(3,"div",2),c.Nc(4,d,12,3,"div",3),c.Nc(5,f,7,0,"div",3),c.Nc(6,m,6,0,"div",3),c.Vb()),2&t&&(c.Bb(3),c.qc("ngSwitch",i.viewState),c.Bb(1),c.qc("ngSwitchCase",1),c.Bb(1),c.qc("ngSwitchCase",2),c.Bb(1),c.qc("ngSwitchCase",3))},directives:[o.i,b.x,b.y,r.b,s.a,b.t,l.c],styles:[""]}),t}()}}]);
//# sourceMappingURL=https://sources.2sxc.org/11.01.00/ng-edit/content-item-import-component.6425e4bdc3d222e5e35d.js.map