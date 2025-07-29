$(document).ready(function() {
    loadJsonDataToStorage()
        .then((items) => populateYearCombobox(items));
});
const UlawAutomationDataKey = 'UlawAutomationData';
const SelectStudentYearComboboxId = "#select-student-year-combobox";
const SelectStudentSemesterComboboxId = "#select-student-semester-combobox";
const SelectStudentCourseComboboxId = "#select-student-course-combobox";
const SelectStudentClassComboboxId = "#select-student-class-combobox";
const ResultTextboxId = "#result-textbox";
 
function populateYearCombobox(items) {
    let idCounter = 1;
    let yearItems = [];
    let years = Array.from(new Set(items.map(x => x.year)));

    for (let year of years) {
        yearItems.push({
            id: idCounter,
            text: year
        });
        
        idCounter++;
    }

    $(SelectStudentYearComboboxId)
        .select2({
            data: yearItems
        })
        .on('select2:select', (e) => {
            onYearComboboxSelected();
        });
}


function populateSemesterCombobox(selectedYear) {
    let items = JSON.parse(localStorage.getItem(UlawAutomationDataKey));
    items = items.filter(x => x.year === selectedYear);
    let idCounter = 1;
    let semesterItems = [];
    let semesters = Array.from(new Set(items.map(x => x.semester)));

    for (let semester of semesters) {
        semesterItems.push({
            id: idCounter,
            text: semester
        });
        
        idCounter++;
    }

    $(SelectStudentSemesterComboboxId)
        .select2({
            data: semesterItems
        })
        .on('select2:select', (e) => {
            onSemesterComboboxSelected();
        });
}

function clearSemesterCombobox() {
    $(SelectStudentSemesterComboboxId).empty().append(new Option());
    $(SelectStudentSemesterComboboxId).val(null);
}

function populateCourseCombobox(selectedYear, selectedSemester) {
    let items = JSON.parse(localStorage.getItem(UlawAutomationDataKey));
    items = items.filter(x => x.year === selectedYear && x.semester === selectedSemester);
    let idCounter = 1;
    let courseItems = [];
    let courses = Array.from(new Set(items.map(x => x.course)));

    for (let course of courses) {
        courseItems.push({
            id: idCounter,
            text: course
        });
        
        idCounter++;
    }

    $(SelectStudentCourseComboboxId)
        .select2({
            data: courseItems
        })
        .on('select2:select', (e) => {
            onCourseComboboxSelected();
        });
}

function clearCourseCombobox() {
    $(SelectStudentCourseComboboxId).empty().append(new Option());
    $(SelectStudentCourseComboboxId).val(null);
}

function populateClassCombobox(selectedYear, selectedSemester, selectedCourse) {
    let items = JSON.parse(localStorage.getItem(UlawAutomationDataKey));
    items = items.filter(x => x.year === selectedYear && x.semester === selectedSemester && x.course === selectedCourse);
    let idCounter = 1;
    let classItems = [];
    let classes = Array.from(new Set(items.map(x => x.fileName)));

    for (let clazz of classes) {
        classItems.push({
            id: idCounter,
            text: clazz
        });
    
        idCounter++;
    }

    $(SelectStudentClassComboboxId)
        .select2({
            data: classItems
        })
        .on('select2:select', (e) => {
            onClassComboboxSelected();
        });
}

function clearClassCombobox() {
    $(SelectStudentClassComboboxId).empty().append(new Option());
    $(SelectStudentClassComboboxId).val(null);
}

function onYearComboboxSelected() {
    clearSemesterCombobox();
    clearCourseCombobox();
    clearClassCombobox();
    clearResultBox();
    const selectedYear = $(SelectStudentYearComboboxId).select2('data')[0].text;
    populateSemesterCombobox(selectedYear);
}

function onSemesterComboboxSelected() {
    clearCourseCombobox();
    clearClassCombobox();
    clearResultBox();
    const selectedYear = $(SelectStudentYearComboboxId).select2('data')[0].text;
    const selectedSemester = $(SelectStudentSemesterComboboxId).select2('data')[0].text;
    populateCourseCombobox(selectedYear, selectedSemester);
}

function onCourseComboboxSelected() {
    clearClassCombobox();
    clearResultBox();
    const selectedYear = $(SelectStudentYearComboboxId).select2('data')[0].text;
    const selectedSemester = $(SelectStudentSemesterComboboxId).select2('data')[0].text;
    const selectedCourse = $(SelectStudentCourseComboboxId).select2('data')[0].text;
    populateClassCombobox(selectedYear, selectedSemester, selectedCourse);
}

function onClassComboboxSelected() {
    const selectedYear = $(SelectStudentYearComboboxId).select2('data')[0].text;
    const selectedSemester = $(SelectStudentSemesterComboboxId).select2('data')[0].text;
    const selectedCourse = $(SelectStudentCourseComboboxId).select2('data')[0].text;
    const selectedClass = $(SelectStudentClassComboboxId).select2('data')[0].text;
    fillResult(selectedYear, selectedSemester, selectedCourse, selectedClass);
}

function fillResult(selectedYear, selectedSemester, selectedCourse, selectedClass) {
    let items = JSON.parse(localStorage.getItem(UlawAutomationDataKey));
    let item = items.find(x => x.year === selectedYear 
        && x.semester === selectedSemester 
        && x.course === selectedCourse
        && x.fileName === selectedClass);
    $(ResultTextboxId).val(item.fileUrl);
}

function clearResultBox() {
    $(ResultTextboxId).val(null);
}

function loadJsonDataToStorage() {
    let gitHubPage = 'https://raw.githubusercontent.com/lpnam0201/TimeTableGenerator/refs/heads/master/docs/data';
    let dataFileNames = [
        "ulaw_automation_khoa46_20252026_hk1.json",
        "ulaw_automation_khoa47_20252026_hk1.json",
        "ulaw_automation_khoa48_20252026_hk1.json",
        "ulaw_automation_khoa49_20252026_hk1.json",
        "ulaw_automation_khoa50_20252026_hk1.json"
    ];
    let loadDataPromises = [];
    for (let dataFileName of dataFileNames) {
        let url = `${gitHubPage}/${dataFileName}`;
        let promise = fetch(url).then(response => response.json());
        loadDataPromises.push(promise);
    }
    let resolved = Promise.all(loadDataPromises);
    return resolved.then((responses) => {
        let items = [];
        for (let response of responses) {
            items.push(...response);
        }
        localStorage.setItem(UlawAutomationDataKey, JSON.stringify(items));
        return items;
    })
}