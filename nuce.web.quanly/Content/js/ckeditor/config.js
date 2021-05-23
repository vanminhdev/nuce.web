/**
 * @license Copyright (c) 2003-2021, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.toolbarGroups = [
		{ name: 'clipboard', groups: ['clipboard', 'undo'] },
		{ name: 'editing', groups: ['find', 'selection', 'spellchecker'] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document', groups: ['mode', 'document', 'doctools'] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
		{ name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi'] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' }
	];
	config.format_tags = 'p;h1;h2;h3;pre';
	config.extraPlugins = 'image2';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

	config.embed_provider = '//ckeditor.iframe.ly/api/oembed?url={url}&callback={callback}';

	//config.filebrowserBrowseUrl = '/ckfinder/ckfinder';
	//config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder?type=Images';
	//config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder?type=Flash';
	config.filebrowserUploadUrl = '/ckfinder/connector?command=QuickUpload&type=Files';
	config.filebrowserImageUploadUrl = '/ckfinder/connector?command=QuickUpload&type=Images';
	//config.filebrowserFlashUploadUrl = '/ckfinder/connector?command=QuickUpload&type=Flash';
};
