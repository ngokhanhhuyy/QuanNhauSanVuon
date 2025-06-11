using System.Reflection;
using QuanNhauSanVuon.Extensions;

namespace QuanNhauSanVuon.Localization;

public static class DisplayNames
{
    // Common.
    public const string Name = "Tên";
    public const string SortedByField = "Sắp xếp theo trường";
    public const string SortedByAscending = "Sắp xếp từ nhỏ đến lớn";
    public const string Page = "Trang";
    public const string ResultsPerPage = "Kết quả mỗi trang";
    public const string CreatedDateTime = "Ngày giờ tạo";
    public const string LastUpdatedDateTime = "Ngày giờ cập nhật lần cuối";

    // User.
    public const string User = "Người dùng";
    public const string UserName = "Tên người dùng";
    public const string Password = "Mật khẩu";

    // Role.
    public const string Role = "Vị trí";
    public const string DisplayName = "Tên hiển thị";
    public const string PowerLevel = "Chỉ số quyền";

    // SeatingAreas.
    public const string SeatingArea = "Khu vực bàn";
    public const string Color = "Màu sắc";
    public const string TakenUpPositions = "Diện tích";

    // Seatings.
    public const string Seating = "Bàn";
    public const string SeatingPosition = "Vị trí bàn";

    // Order.
    public const string Order = "Order";
    public const string NetAmount = "Giá tiền trước thuế";
    public const string VatAmount = "Tiền thuế";
    public const string GrossAmount = "Giá tiền sau thuế";
    public const string PaidDateTime = "Ngày giờ thanh toán";

    // OrderItem.
    public const string OrderItem = "Món/đồ uống được order";
    public const string OrderItemList = "Danh sách món/đồ uống được order";
    public const string Quantity = "Số lượng";
    public const string NetPricePerUnit = "Giá tiền mỗi đơn vị";
    public const string VatAmountPerUnit = "Tiền thuế mỗi đơn vị";
    public const string OrderedDateTime = "Ngày giờ order";

    // MenuItems.
    public const string MenuItem = "Món/đồ uống";
    public const string MenuItemName = "Tên món/đồ uống";
    public const string DefaultAmount = "Giá mặc định";
    public const string DefaultVatPercentage = "Tỉ lệ thuế VAT mặc định";
    public const string Unit = "Đơn vị";
    public const string ThumbnailUrl = "Địa chỉ ảnh đại diện";

    // MenuCategory.
    public const string MenuCategory = "Phân loại menu";
    public const string MenuCategoryName = "Tên phân loại menu";

    // MonthYear.
    public const string Month = "Tháng";
    public const string Year = "Năm";

    private static readonly Dictionary<string, string> _names;

    static DisplayNames()
    {
        _names = new Dictionary<string, string>();
        BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Static;
        FieldInfo[] fields = typeof(DisplayNames).GetFields(bindingFlags);

        foreach (var field in fields)
        {
            _names.Add(field.Name, (string)field.GetValue(null));
        }
    }

    public static string Get(string objectName)
    {
        ArgumentNullException.ThrowIfNull(objectName);
        return _names
            .Where(pair => pair.Key == objectName.UpperCaseFirstLetter())
            .Select(pair => pair.Value)
            .SingleOrDefault()
            ?? throw new InvalidOperationException(
                $"There is no display name for {objectName}");
    }

    public static Dictionary<string, string> GetAll()
    {
        return _names;
    }
}