Generate workout with one set of every possible exercise.
`GenerateEveryPossibleExercise`
Then upload to garmin, view in browser.
Use JS console, run this snippet, grab the resulting JS object.

```javascript
(() => {
    const subtypesPerCategory = 200;
    const exerciseNames = Array.from(document.querySelectorAll('td[data-title-2]')).map(v => v.innerText);
    const exerciseCategories = window.exerciseCategories = Object.create(null);
    for(let i = 0; i < exerciseNames.length / subtypesPerCategory; i++) {
        const slice = exerciseNames.slice(i * subtypesPerCategory, subtypesPerCategory);
        const categoryName = slice[slice.length - 1];
        let j = subtypesPerCategory - 1;
        while(slice[j] == categoryName) {j--}
        while(slice[j] == 'Choose an Exercise') {j--}
        exerciseCategories[categoryName] = slice.slice(0, j + 1);
    }
    // Copy JSON to clipboard
    copy(exerciseCategories);
})();
```
