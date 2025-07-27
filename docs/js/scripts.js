$(document).ready(function() {
    initializeComboboxes();
    loadJsonDataToStorage();
});
const UlawAutomationDataKey = 'UlawAutomationData';

function initializeComboboxes() {
    let comboboxClasses = [
        'select-student-year',
        'select-student-term',
        'select-student-course',
        'select-student-class',
    ];
    for (let comboboxClass of comboboxClasses) {
        $(comboboxClass).select2();
    }
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
    resolved.then((responses) => {
        let items = [];
        for (let response of responses) {
            items.push(...response);
        }
        localStorage.setItem(UlawAutomationDataKey, JSON.stringify(items));
    })
}