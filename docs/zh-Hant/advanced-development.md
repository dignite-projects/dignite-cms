# 進階開發

## 按字段值查詢項目

### `cms-entry-list`

`cms-entry-list` 有一個按字段值查詢項目的參數(`querying-by-fields`)，該參數是`QueryingByField`類實例的列表，`QueryingByField`包含兩個參數：

- `Name`：字段名稱
- `Value`：用於查詢的字段值，根據字段的不同，該值有不同的形式：

  - `TextFieldQuerying`：在字段中進行是否包含`Value`方式過濾。
  - `SwitchFieldQuerying`：`Value`必須可以轉換為`bool`型，判斷值是否等於`Value`的方式過濾。
  - `NumericFieldQuerying`：`Value`使用`-`分隔最小值和最大值，過濾字段值大於`Value`最小值和小於最大值。
  - `SelectFieldQuerying`：`Value`使用`,`分隔多個`Guid`值，過濾字段值是否含有`Value`中的`Guid`值。
  - `EntryFieldQuerying`：`Value`使用`,`分隔多個`Guid`值，過濾字段值是否含有`Value`中的`Guid`值。

    > 更多支持的查詢方式在未來版本中提供。

### `GetListAsync(GetEntriesInput input)` 方法

在`IEntryPublicAppService` `GetListAsync(GetEntriesInput input)`方法中有一個名為`QueryingByFieldsJson`字符串參數，該參數是`QueryingByField`列表序列化形式。

事實上`cms-entry-list`內部是將`QueryingByField`列表序列化為JSON，傳遞給`IEntryPublicAppService` `GetListAsync(GetEntriesInput input)`方法中的`QueryingByFieldsJson`參數。
