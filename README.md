

```markdown
# Bùi Hoàng Thanh Lâm - 2221050435

## 1. Cấu trúc thư mục dự án .NET MVC

Thiết kế theo mẫu kiến trúc **Model-View-Controller**.
Ví dụ `MyMvc/`:

```text
MyMvc/
├── Controllers           <-- Nơi chứa logic điều khiển
├── Models                <-- Nơi chứa cấu trúc dữ liệu
├── Views                 <-- Nơi chứa giao diện (HTML/Razor)
├── wwwroot               <-- Nơi chứa file tĩnh (CSS, JS, Images)
├── Properties            <-- Cấu hình khởi chạy (launchSettings.json)
├── appsettings.json      <-- Cấu hình ứng dụng (DB string, API keys)
├── Program.cs            <-- Điểm khởi chạy ứng dụng (Entry Point)
└── MyMvcProject.csproj   <-- File định nghĩa dự án

```

---

## 2. Định tuyến (Route) trong .NET MVC

Trong ASP.NET Core MVC, **Routing** (Định tuyến) là cơ chế chịu trách nhiệm ánh xạ các yêu cầu URL từ trình duyệt (ví dụ: `/Product/Detail/5`) vào đúng Controller và Action cụ thể để xử lý.

Có 2 loại định tuyến chính:

### 2.1. Conventional Routing (Định tuyến theo quy ước)

Cấu hình trong `Program.cs`:

```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

```

* **{controller}**: Phân đoạn đầu tiên của URL sẽ được hiểu là tên Controller. Nếu URL trống, mặc định gọi `HomeController`.
* **{action}**: Phân đoạn thứ hai là tên hàm (Action) trong Controller đó. Nếu thiếu, mặc định gọi hàm `Index`.
* **{id?}**: Phân đoạn thứ ba là tham số (ví dụ: ID sản phẩm). Dấu chấm hỏi `?` nghĩa là tham số này không bắt buộc (Optional).

### 2.2. Attribute Routing (Định tuyến bằng thuộc tính)

Đặt trực tiếp trên Controller hoặc Action:

```csharp
[Route("san-pham")] // Đặt URL gốc cho controller này là /san-pham
public class ProductController : Controller
{
    // URL: /san-pham/danh-sach
    [Route("danh-sach")]
    public IActionResult List()
    {
        return View();
    }

    // URL: /san-pham/chi-tiet/5
    [HttpGet("chi-tiet/{id}")]
    public IActionResult Detail(int id)
    {
        return View();
    }
}

```

### 2.3. Route Constraints (Ràng buộc dữ liệu)

* **Cú pháp:** `{tham_số:ràng_buộc}`
* **Ví dụ:** Bạn chỉ muốn ID là số nguyên.
* Conventional: pattern: `"{controller}/{action}/{id:int}"`
* Attribute: `[HttpGet("profile/{id:int}")]`


* **Các ràng buộc phổ biến:**
* `{id:int}`: Phải là số nguyên.
* `{id:min(1)}`: Số nguyên tối thiểu là 1.
* `{slug:maxlength(50)}`: Chuỗi dài tối đa 50 ký tự.
* `{active:bool}`: Phải là true/false.



---

## 3. Controller và View trong ASP.NET Core MVC

### 3.1. Controller (Bộ điều khiển)

Controller là thành phần chịu trách nhiệm xử lý các yêu cầu (requests) từ người dùng. Nó đóng vai trò là "nhạc trưởng", điều phối luồng dữ liệu và quyết định phản hồi nào sẽ được gửi về trình duyệt.

#### Đặc điểm chính

* Thường nằm trong thư mục `Controllers`.
* Là một class C# kế thừa từ lớp cơ sở `Microsoft.AspNetCore.Mvc.Controller`.
* Tên class phải có hậu tố `Controller` (ví dụ: `HomeController`, `ProductController`).

#### Action Methods (Hành động)

Các phương thức `public` bên trong Controller được gọi là **Actions**. Mỗi Action tương ứng với một URL cụ thể.

**Các loại kết quả trả về (Return Types) phổ biến:**

| Kiểu trả về | Phương thức gọi | Mô tả |
| --- | --- | --- |
| `ViewResult` | `return View();` | Trả về một trang HTML (View). |
| `JsonResult` | `return Json(data);` | Trả về dữ liệu dạng JSON (thường dùng cho API/AJAX). |
| `RedirectToActionResult` | `return RedirectToAction();` | Chuyển hướng sang một Action khác. |
| `ContentResult` | `return Content("Text");` | Trả về chuỗi văn bản thuần túy. |
| `IActionResult` | (Linh động) | Interface cha, cho phép trả về bất kỳ loại nào ở trên. |

#### Ví dụ Code Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using MyProject.Models; // Namespace chứa Model

public class ProductController : Controller
{
    // URL: /Product/Index
    public IActionResult Index()
    {
        // Logic giả lập lấy dữ liệu
        var products = new List<string> { "Laptop", "Mouse", "Keyboard" };      
        
        // Truyền dữ liệu sang View bằng ViewBag
        ViewBag.Message = "Danh sách sản phẩm mới nhất";     
        
        // Trả về View tương ứng (Views/Product/Index.cshtml) cùng với Model
        return View(products); 
    }

    // URL: /Product/Detail/5
    public IActionResult Detail(int id)
    {
        if (id == 0)
        {
            return NotFound(); // Trả về lỗi 404
        }
        return Content($"Đang xem sản phẩm có ID: {id}");
    }
}

```

### 3.2. View (Giao diện)

View là thành phần chịu trách nhiệm hiển thị dữ liệu cho người dùng. Trong ASP.NET Core, View sử dụng **Razor View Engine** (đuôi file `.cshtml`), cho phép viết mã C# lồng trực tiếp vào HTML.

#### Đặc điểm chính

* Nằm trong thư mục `Views/[Tên_Controller]/`.
* Sử dụng ký tự `@` để chuyển từ HTML sang C#.
* Không nên chứa logic nghiệp vụ phức tạp (logic nên nằm ở Controller).

#### Các cách truyền dữ liệu từ Controller sang View

Có 3 cách chính để Controller gửi dữ liệu hiển thị:

1. **ViewData:** Kiểu `Dictionary` (Key-Value), yêu cầu ép kiểu khi dùng.
2. **ViewBag:** Kiểu `Dynamic` (tiện lợi nhưng không kiểm tra lỗi lúc code - compile time).
3. **Strongly Typed Model (Khuyên dùng):** Truyền một object cụ thể, hỗ trợ nhắc lệnh (IntelliSense) và kiểm tra lỗi tốt.

### 3.3. Mối quan hệ và Luồng đi (Workflow)

Quá trình xử lý một yêu cầu diễn ra như sau:

1. **Request:** Người dùng truy cập URL `/Product/Index`.
2. **Routing:** Hệ thống định tuyến tìm đến `ProductController` và hàm `Index()`.
3. **Controller:**
* Hàm `Index()` chạy.
* Chuẩn bị dữ liệu (ví dụ: list sản phẩm).
* Gọi lệnh `return View(data)`.


4. **View:**
* Nhận dữ liệu thông qua `@model`.
* Razor Engine biên dịch mã `.cshtml` thành HTML thuần.


5. **Response:** Trả HTML về trình duyệt người dùng.

```

```

1. Tìm hiểu về ViewBag trong MVC
ViewBag là một đối tượng động (dynamic object) được sử dụng để truyền dữ liệu từ Controller sang View.

Đặc điểm: Nó cho phép bạn thêm các thuộc tính tùy ý vào đối tượng mà không cần định nghĩa trước (ví dụ: ViewBag.Title, ViewBag.MyData).

Phạm vi: Dữ liệu trong ViewBag chỉ tồn tại trong suốt quá trình xử lý Request hiện tại. Nếu có một lệnh chuyển hướng (Redirect), dữ liệu trong ViewBag sẽ bị xóa sạch.

Cú pháp:

Trong Controller: ViewBag.PropertyName = value;

Trong View (Razor): @ViewBag.PropertyName
Chào bạn! Rất vui được đồng hành cùng bạn trong lộ trình chinh phục ASP.NET MVC. Dưới đây là hướng dẫn chi tiết cho các nội dung bạn yêu cầu để nắm vững luồng xử lý dữ liệu trong MVC.

1. Tìm hiểu về ViewBag trong MVC
ViewBag là một đối tượng động (dynamic object) được sử dụng để truyền dữ liệu từ Controller sang View.

Đặc điểm: Nó cho phép bạn thêm các thuộc tính tùy ý vào đối tượng mà không cần định nghĩa trước (ví dụ: ViewBag.Title, ViewBag.MyData).

Phạm vi: Dữ liệu trong ViewBag chỉ tồn tại trong suốt quá trình xử lý Request hiện tại. Nếu có một lệnh chuyển hướng (Redirect), dữ liệu trong ViewBag sẽ bị xóa sạch.

Cú pháp:

Trong Controller: ViewBag.PropertyName = value;

Trong View (Razor): @ViewBag.PropertyName


<h2>Thông báo: @ViewBag.WelcomeMessage</h2>
<p>Bây giờ là: @ViewBag.CurrentTime</p>
3. Gửi nhận dữ liệu qua Form Submit
Trong MVC, quá trình gửi nhận dữ liệu qua Form thường diễn ra như sau:

View: Chứa thẻ <form> với phương thức method="post". Các ô nhập liệu (<input>) phải có thuộc tính name trùng với tên tham số hoặc thuộc tính trong Model.

Controller: Sử dụng Action được đánh dấu thuộc tính [HttpPost] để nhận dữ liệu khi người dùng nhấn nút Submit.