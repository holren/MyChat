using Supabase;
using Postgrest.Attributes;
using System;

namespace MyChat.Model
{
public class users : SupabaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("age")]
        public int Age { get; set; } = 0;
    } 
}
