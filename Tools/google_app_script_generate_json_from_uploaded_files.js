function myFunction() {
  
  
  
  let folderIterator = DriveApp.getFoldersByName("ulaw_automation_khoa50_20252026_hk1");

  let fileList = [];
  let course = "50";
  let semester = "HK1";
  let year = "2025-2026";
  while (folderIterator.hasNext()) {
    let folder = folderIterator.next();
    let fileIterator = folder.getFiles();
    while (fileIterator.hasNext()) {
      let file = fileIterator.next();
      fileList.push({
        fileName: file.getName(),
        fileUrl: file.getUrl(),
        course: course,
        semester: semester,
        year: year
      })
    }
  }
  let bigText = JSON.stringify(fileList);
  let middle = bigText.length / 2;
  let firstPart = bigText.substring(0, middle);
  let secondPart = bigText.substring(middle, bigText.length);
  console.log(firstPart);
  console.log(secondPart);
}
