// Use VSCode "Typescript JSON schema generator"
// https://marketplace.visualstudio.com/items?itemName=marcoq.vscode-typescript-to-json-schema
// to convert to schema

export interface Workout {
    title: string;
    type: 'Cardio' | 'Strength' | 'Yoga' | 'Pilates';
    exercises: Array<Exercise | SuperSet>;
}
export interface SuperSet {
    repeat: number;
    exercises: Array<Exercise>;
}
export interface Exercise {
    name: ExerciseName;
    /**
     * If you're using a name that garmin doesn't recognize,
     * your watch will show it but Garmin Connect will not.
     * Set this value to an official name for Garmin Connect.
     */
    garminName?: ExerciseName;
    notes?: string;
    reps?: number;
    duration?: string;
    rest?: string;
}
// Post-process: replace in the schema
export type ExerciseName = string;
