using System;
using System.Collections.Generic;

namespace WebApplication1.DB;

public partial class Credential
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;
}
