# Bộ dự án ứng tuyển vị trí .Net developer

ASP.Net Web API, WPF

- Database: Sqlite
- Back-end: Web API
- Front-end: WPF

- Mặc dù quen với việc xây dựng database sử dụng Postgres, nhưng nếu dùng nó thì có thể gây khó khăn cho người đánh giá + là test project nên không quá đặt nặng an toàn database, vì vậy Sqlite đã được chọn. File database nằm trong root folder của web-api project với tên database.db. Có thể sử dụng tools (DBeauver...) để kết nối vào đây và theo dõi cấu trúc database
- Back-end sử dụng nền tảng ASP.NET Web API, có áp dụng Entity Framework vào để thực hiện các tác vụ với database. Back-end này phục vụ luôn cho việc xây dựng WPF app, nghĩa là WPF app là 1 client, sử dụng các API từ back-end đã xây dựng, không trực tiếp access database
- Theo yêu cầu bài 3, tôi quyết định chọn WPF để thực hiện.
- Framework version là .Net 8.0.200

## Cách chạy để kiểm tra kết quả

- Kiểm tra để cài đặt môi trường (.net 8) nếu cần
- Mở WebAPI project và chạy để run back-end. Có thể sử dụng cli (dotnet run) hoặc start trên Visual Studio với chế độ http.
  Sau khi chạy, sẽ cho phép backend hoạt động ở http://localhost:7001. Đây chính là cổng mà WPF app dùng để sử dụng APIs của back-end.
  Sau khi hoàn thành, trình duyệt sẽ tự động mở tại địa chỉ http://localhost:7001/swagger/index.html. Có thể xem kết quả web-api tại đây.
  Nhưng đa phần yêu cầu xác thực nên hầu như không thể gọi api trực tiếp tại đây
- Mở AdminApp để run bằng lệnh cli hoặc editor
- Sử dụng tài khoản sau để sử dụng ứng dụng với quyền admin (full quyền): admin | 123
