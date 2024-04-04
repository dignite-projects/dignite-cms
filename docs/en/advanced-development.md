# Advanced Development

## Querying Entries by Fields

### `cms-entry-list`

The `cms-entry-list` component has a parameter for querying entries by field values (`querying-by-fields`), which is a list of instances of the `QueryingByField` class. Each `QueryingByField` instance contains two parameters:

- `Name`: The name of the field.
- `Value`: The field value used for querying. Depending on the type of field, the value takes different forms:

  - `TextFieldQuerying`: Filters by whether the field contains the specified `Value`.
  - `SwitchFieldQuerying`: The `Value` must be convertible to a `bool`, and filters by whether the value equals the specified `Value`.
  - `NumericFieldQuerying`: The `Value` is formatted as `minValue-maxValue`, filtering by whether the field value is greater than `minValue` and less than `maxValue`.
  - `SelectFieldQuerying`: The `Value` is a comma-separated list of `Guid` values, filtering by whether the field value contains any of the `Guid` values in `Value`.
  - `EntryFieldQuerying`: The `Value` is a comma-separated list of `Guid` values, filtering by whether the field value contains any of the `Guid` values in `Value`.

    > More supported querying methods will be provided in future versions.

### `GetListAsync(GetEntriesInput input)` Method

The `GetListAsync(GetEntriesInput input)` method in `IEntryPublicAppService` accepts a string parameter named `QueryingByFieldsJson`, which represents a serialized list of `QueryingByField` objects.

In fact, internally, the `cms-entry-list` component serializes the list of `QueryingByField` objects into JSON and passes it to the `QueryingByFieldsJson` parameter of the `GetListAsync(GetEntriesInput input)` method in `IEntryPublicAppService`.
