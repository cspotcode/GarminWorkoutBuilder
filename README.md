## References

Workout type <-> Device Compatibility: https://support.garmin.com/en-US/?faq=lLvhWrmlMv0vGmyGpWjOX6

https://www.fitfileviewer.com/

Don't trust garmin's own fit2csv stuff from SDK; it doesn't understand all fields and strips them.

## Building

Requires Garmin FIT SDK: https://developer.garmin.com/fit/download/

```
dotnet workload install wasm-tools
dotnet add package Microsoft.TypeScript.MSBuild
```
