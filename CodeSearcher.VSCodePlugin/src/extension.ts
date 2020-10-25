// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
import * as rm from 'typed-rest-client';


interface Index {
	id: number,
	sourcePath: string,
	indexPath: string,
	createdTime: unknown,
	fileExtensions: string[]
}

interface searchParameters {
	indexID: number,
	searchWord: string
}

interface Finding{
	lineNumber: number,
	position: number,
	length: number
}

interface SearchResult {
	filename: string,
	findings: Finding []
}


let selectedSearchIndex: number = 0;
let listOfSearchResults: SearchResult[];

function activateSelectIndex(context: vscode.ExtensionContext) {
	console.log('Adding search command for "codesearcher-vscodeplugin"');
	listOfSearchResults = [];
	
	let searchDisposable = vscode.commands.registerCommand('codesearcher-vscodeplugin.selectIndex', async () => {

		let rest: rm.RestClient = new rm.RestClient('codeSearcher-vs-plugin', 'http://127.0.0.1:5000');

		const codeSearcher: rm.IRestResponse<Index[]> = await rest.get<Index[]>('/api/CodeSearcher/indexList');
		if(codeSearcher.statusCode == 200) {
			const indexList = codeSearcher.result ?? [];
			var entries:Array<string>=[];
			indexList.forEach((entry)=>{
				entries.push(`${entry.id}::${entry.sourcePath}`)
			})
			
			const selectedItem = await vscode.window.showQuickPick(entries);	
			if(selectedItem){
				const index = entries.indexOf(selectedItem);
				vscode.window.showInformationMessage(`Choosen index: (${indexList[index].id}) - [${indexList[index].fileExtensions}] - ${indexList[index].sourcePath}`);
				selectedSearchIndex = indexList[index].id;
			}
		}
		
	});

	context.subscriptions.push(searchDisposable);
}

function activateSearch(context: vscode.ExtensionContext) {
	console.log('Adding search command for "codesearcher-vscodeplugin"');
	listOfSearchResults = [];
	
	let searchDisposable = vscode.commands.registerCommand('codesearcher-vscodeplugin.search', async () => {

		let rest: rm.RestClient = new rm.RestClient('codeSearcher-vs-plugin', 'http://127.0.0.1:5000');

		if(selectedSearchIndex == 0)
		{
			const codeSearcher: rm.IRestResponse<Index[]> = await rest.get<Index[]>('/api/CodeSearcher/indexList');
			if(codeSearcher.statusCode == 200) {
				const indexList = codeSearcher.result ?? [];
				var entries:Array<string>=[];
				indexList.forEach((entry)=>{
					entries.push(`${entry.id}::${entry.sourcePath}`)
				})
				
				const selectedItem = await vscode.window.showQuickPick(entries);	
				if(selectedItem){
					const index = entries.indexOf(selectedItem);
					vscode.window.showInformationMessage(`Choosen index: (${indexList[index].id}) - [${indexList[index].fileExtensions}] - ${indexList[index].sourcePath}`);
					selectedSearchIndex = indexList[index].id;
				}
			}
		}

		if(selectedSearchIndex != 0) {
			listOfSearchResults = new Array<SearchResult>();
			const searchTerm = await vscode.window.showInputBox({
				placeHolder: 'Here you can enter what you are looking for!'
				// add validation and stuff like that here as well, for example lookups in the search database ...
			});

			let res: rm.IRestResponse<SearchResult[]> = await rest.create<SearchResult[]>('/api/CodeSearcher/search', { indexID: selectedSearchIndex, searchWord: searchTerm });

			res?.result?.forEach((resultEntry)=>{
				listOfSearchResults.push(resultEntry);
			})
			
			vscode.window.showInformationMessage(`Found at least ${listOfSearchResults.length} entries for search term '${searchTerm}'`);
		}
	});

	context.subscriptions.push(searchDisposable);
}


// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

	// Use the console to output diagnostic information (console.log) and errors (console.error)
	// This line of code will only be executed once when your extension is activated
	console.log('Congratulations, your extension "codesearcher-vscodeplugin" is now active!');

	activateSearch(context);
	activateSelectIndex(context);
}

// this method is called when your extension is deactivated
export function deactivate() {}
