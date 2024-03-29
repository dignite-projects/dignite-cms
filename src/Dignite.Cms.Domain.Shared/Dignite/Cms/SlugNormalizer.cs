﻿using Slugify;
using Unidecode.NET;

namespace Dignite.Cms;

public static class SlugNormalizer
{
    static readonly SlugHelper SlugHelper = new();

    public static string Normalize(string value)
    {
        return SlugHelper.GenerateSlug(value?.Unidecode());
    }
}
