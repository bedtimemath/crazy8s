﻿using SC.Common.Extensions;

namespace C8S.WordPress.Abstractions.Extensions;

public static class StringEx
{
    public static string ToSlug(string input) =>
        input.RemoveNonAlphanumeric().ToLower();
}