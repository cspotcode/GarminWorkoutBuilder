https://cspotcode.com/GarminWorkoutBuilder

## Goals

Described in [Home.razor](GarminWorkoutBuilder/Pages/Home.razor)

Also I learned some Blazor.

## References

Workout type <-> Device Compatibility: https://support.garmin.com/en-US/?faq=lLvhWrmlMv0vGmyGpWjOX6

https://www.fitfileviewer.com/

Don't trust garmin's own fit2csv stuff from SDK; it doesn't understand all fields and strips them.

## Building

Requires Garmin FIT SDK: https://developer.garmin.com/fit/download/

```shell
# Necessary for fully client-side app
dotnet workload install wasm-tools

# Not needed anymore
# dotnet add package Microsoft.TypeScript.MSBuild

cd GarminWorkoutBuilder
npm install
```
