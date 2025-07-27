$(document).ready(function() {
    let comboboxClasses = [
        'select-student-year',
        'select-student-term',
        'select-student-course',
        'select-student-class',
    ];
    for (let comboboxClass of comboboxClasses) {
        $(comboboxClass).select2();
    }
});