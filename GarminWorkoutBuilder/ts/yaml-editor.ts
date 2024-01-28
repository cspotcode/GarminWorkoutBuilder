import type * as monacoType from 'monaco-editor';
// const monaco = (window as any).monaco as typeof monacoType;
// @ts-ignore
import EditorWorker from 'monaco-editor/esm/vs/editor/editor.worker?worker';
import { configureMonacoYaml } from 'monaco-yaml';
// @ts-ignore
import YamlWorker from 'monaco-yaml/yaml.worker?worker';
import builtSchema from './schema/built.json';
import workouts from '../../docs/workouts.json';

declare global {
    interface Window {
        monaco: typeof monacoType
        blazorMonaco: {

        }
    }
}

window.MonacoEnvironment = {
    getWorker(moduleId, label) {
      const url = location.href;
      switch (label) {
        case 'editorWorkerService':
          return new Worker(new URL('_content/BlazorMonaco/lib/monaco-editor/min/vs/base/worker/workerMain.js', url))
        case 'json':
          return new Worker(
            new URL('_content/BlazorMonaco/lib/monaco-editor/min/vs/language/json/jsonWorker.js', url)
          )
        case 'yaml':
          return new Worker(new URL('js/node_modules/monaco-yaml/yaml.worker.js', url))
        default:
          throw new Error(`Unknown label ${label}`)
      }
    }
}

const schema = structuredClone(builtSchema);
const workoutNames = [];
for(const [key, value] of Object.entries(workouts)) {
    workoutNames.push(key);
    workoutNames.push(...value);
}
schema.definitions.ExerciseName = {
    anyOf: [
        {enum: workoutNames},
        {type: 'string'}
    ]
};

function configureMonaco() {
    const {monaco} = window;
    if(!monaco) {
        setTimeout(configureMonaco, 10);
        return;
    }
    configureMonacoYaml(monaco, {
        enableSchemaRequest: false,
        schemas: [
            {
                // If YAML file is opened matching this glob
                fileMatch: ['file:///workout.yaml'],
                // And the following URI will be linked to as the source.
                uri: 'http://example.com/workout.json',
                // The following schema will be applied
                schema
            }
        ]
    });

    configureMonacoEditor();
}

function configureMonacoEditor() {
    const {monaco} = window;
    const editor = monaco.editor.getEditors()[0];
    if(!editor) {
        setTimeout(configureMonacoEditor, 10);
        return;
    }
    monaco.editor.getEditors()[0].setModel(
        monaco.editor.createModel(
            '',
            'yaml',
            monaco.Uri.parse('file:///workout.yaml')
        )
    );
}

document.addEventListener("DOMContentLoaded", function(event) {
    configureMonaco();
});