﻿using System;
using System.Collections.Generic;

namespace Entities;

public partial class Category
{
    //Hey! It's Tamar...
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
