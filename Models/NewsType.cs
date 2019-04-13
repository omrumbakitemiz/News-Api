using System.Runtime.Serialization;

namespace News.Api.Models
{
    public enum NewsType
    {
        [EnumMember(Value = "World")]
        World,
        
        [EnumMember(Value = "Turkey")]
        Turkey,
        
        [EnumMember(Value = "Politics")]
        Politics,
        
        [EnumMember(Value = "Business")]
        Business,
        
        [EnumMember(Value = "Tech")]
        Tech,
        
        [EnumMember(Value = "Science")]
        Science,
        
        [EnumMember(Value = "Sport")]
        Sport,
        
        [EnumMember(Value = "Education")]
        Education,
        
        [EnumMember(Value = "Economy")]
        Economy,
    }
}