# TimeTableGenerator

Tool giúp dịch thời khóa biểu SV ĐH Luật HCM.
Bản web ở đây: https://lpnam0201.github.io/TimeTableGenerator/

# Cách sử dụng
Link tải: 

32-bit: https://github.com/lpnam0201/TimeTableGenerator/releases/download/1.1/TimeTableGenerator-1.1-x86.zip<br />
64-bit: https://github.com/lpnam0201/TimeTableGenerator/releases/download/1.1/TimeTableGenerator-1.1-x64.zip

1. Giải nén
2. Mở chương trình Command Prompt trên Windows (gõ CMD vào phần tìm kiếm của Windows -> chọn Command Prompt)
3. Trên cửa sổ Command Prompt, gõ lệnh
  > cd đường-dẫn-đến-thư-mục-đã-giải-nén-ở-bước-1<br />
  > Ví dụ: cd C:\Users\lpnam\Desktop\win-x64
4. Nhấn Enter
5. Trên cửa sổ Command Prompt, gõ lệnh:
  > TimeTableGenerator.exe --FilePath "C:\Users\lpnam\Desktop\Khoa-47.xls" --SheetName "149-TMQT47" --DiscussionGroup "1"

   Trong này:<br />
   Theo sau `--FilePath` là đường dẫn đến file cần dịch<br />
   Theo sau `--SheetName` là tên sheet trong file tương ứng với lớp của bạn (mình học 149-TMQT47 nên sẽ ghi là `--SheetName "149-TMQT47"`)<br />
   Theo sau `--DiscussionGroup` là "1" hoặc "2", tùy nhóm thảo luận của bạn là "1" hay "2"

6. Nhấn Enter
7. Xem kết quả ở file result.xls trong thư mục giải nén

Mọi thắc mắc xin tạo 1 issue ở link này (https://github.com/lpnam0201/TimeTableGenerator/issues) để được giải đáp!
