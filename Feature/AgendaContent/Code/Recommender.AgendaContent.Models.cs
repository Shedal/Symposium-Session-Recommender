#pragma warning disable 1591
#pragma warning disable 0108
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Team Development for Sitecore.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;   
using System.Collections.Generic;   
using System.Collections.Specialized; 
using System.Linq;
using System.Text;
using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Fields;
using Sitecore.Globalization;
using Sitecore.Data.Items;


namespace Recommender.AgendaContent
{
	using global::System.IO;
	using Sitecore.Data;
	using Sitecore.Data.Items;

	public partial interface IGlassBase 
	{
		[SitecoreId]
		Guid Id { get; }

		[SitecoreInfo(SitecoreInfoType.Language)]
        Language Language { get; }
		
        [SitecoreInfo(SitecoreInfoType.Version)]
        int Version { get; }

        [SitecoreInfo(SitecoreInfoType.Url)]
        string Url { get; }

        [SitecoreInfo(SitecoreInfoType.Name)]
        string Name { get; }

		[SitecoreInfo(SitecoreInfoType.ContentPath)]
		string Path { get; }
		
		[SitecoreInfo(SitecoreInfoType.FullPath)]
		string FullPath { get; }
		
		[SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        IEnumerable<Guid> BaseTemplateIds { get; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        Guid ItemTemplateId { get; }

		[SitecoreItem]
        Item Item { get; }
	
	    [SitecoreChildren]
	    IEnumerable<Item> Children { get; }

        bool InheritsTemplate(Guid templateGuid);

		bool InheritsTemplate(ID templateId);
	}

	public abstract partial class GlassBase : IGlassBase 
	{
		[SitecoreId]
		public virtual Guid Id{ get; private set; }

		[SitecoreInfo(SitecoreInfoType.Language)]
        public virtual Language Language{ get; private set; }

        [SitecoreInfo(SitecoreInfoType.Version)]
        public virtual int Version { get; private set; }

		[SitecoreInfo(SitecoreInfoType.Url)]
        public virtual string Url { get; private set;}
		
        [SitecoreInfo(SitecoreInfoType.Name)]
        public virtual string Name { get; private set;}

		[SitecoreInfo(SitecoreInfoType.ContentPath)]
		public virtual string Path { get; private set; }

		[SitecoreInfo(SitecoreInfoType.FullPath)]
		public virtual string FullPath { get; private set; }
		
		[SitecoreItem]
        public virtual Item Item { get; private set; }
	
	    [SitecoreChildren]
	    public virtual IEnumerable<Item> Children { get; private set; }
		
		[SitecoreInfo(SitecoreInfoType.BaseTemplateIds)]
        public virtual IEnumerable<Guid> BaseTemplateIds { get; private set; }

        [SitecoreInfo(SitecoreInfoType.TemplateId)]
        public virtual Guid ItemTemplateId { get; private set; }

        /// <summary>
        /// Returns a bool indicating if the item inherits the given template
        /// </summary>
        /// <param name="templateGuid">Template Guid</param>
        /// <returns>True if the item is or inherits from a template</returns>
        public bool InheritsTemplate(Guid templateGuid)
        {
            return this.BaseTemplateIds.Contains(templateGuid) || this.ItemTemplateId.Equals(templateGuid);
        }

		/// <summary>
        /// Returns a bool indicating if the item inherits the given template
        /// </summary>
        /// <param name="templateId">Template ID</param>
        /// <returns>True if the item is or inherits from a template</returns>
		public bool InheritsTemplate(ID templateId)
		{
			return this.BaseTemplateIds.Contains(templateId.Guid) || this.ItemTemplateId.Equals(templateId.Guid);
		}
	}
	
    [SitecoreType(TemplateId = "{962B53C4-F93B-4DF9-9821-415C867B8903}")]
	public class MediaFile : GlassBase {
	
		[SitecoreInfo(SitecoreInfoType.MediaUrl)]
        public virtual string Url { get; private set;}
   
		[SitecoreField("Attachment")]
		public virtual Stream File { get; set; }
		
		[SitecoreField("File Path")]
        public virtual string FilePath { get; set; }

        [SitecoreField("Mime Type")]
        public virtual string MimeType { get; set; }

        [SitecoreField("Title")]
        public virtual string Title { get; set; }

        [SitecoreField("Format")]
        public virtual string Format { get; set; }

        [SitecoreField("Description")]
        public virtual string Description { get; set; }
		
        [SitecoreField("Keywords")]
        public virtual string Keywords { get; set; }

        [SitecoreField("Extension")]
        public virtual string Extension { get; set; }
	}
	
    [SitecoreType(TemplateId = "{6D1CD897-1936-4A3A-A511-289A94C2A7B1}")]
	public class DictionaryEntry : GlassBase 
	{
		[SitecoreField("Key")]
        public virtual string Key { get; private set;}
   
		[SitecoreField("Phrase")]
		public virtual string Phrase { get; set; }
	}
}
﻿














namespace Recommender.AgendaContent
{
    using Sitecore.Data.Items;
    using Sitecore.Data;

 	/// <summary>
	/// ILabel Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Sym2018/Feature/Agenda/Label</para>	
	/// <para>ID: 81ef8a20-7354-4d77-ba6e-a834fba8ec0b</para>	
	/// </summary>
	[SitecoreType(TemplateId="81ef8a20-7354-4d77-ba6e-a834fba8ec0b")] //, Cachable = true
	public partial interface ILabel : IGlassBase 
	{
				}

	
	/// <summary>
	/// Label
	/// <para></para>
	/// <para>Path: /sitecore/templates/Sym2018/Feature/Agenda/Label</para>	
	/// <para>ID: 81ef8a20-7354-4d77-ba6e-a834fba8ec0b</para>	
	/// </summary>
	[SitecoreType(TemplateId="81ef8a20-7354-4d77-ba6e-a834fba8ec0b")] //, Cachable = true
	public partial class Label  : GlassBase, ILabel 
	{
			public const string TemplateIdString = "81ef8a20-7354-4d77-ba6e-a834fba8ec0b";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Label";

			
	   
			
	}
}
﻿














namespace Recommender.AgendaContent
{
    using Sitecore.Data.Items;
    using Sitecore.Data;

 	/// <summary>
	/// ISymposiumSession Interface
	/// <para></para>
	/// <para>Path: /sitecore/templates/Sym2018/Feature/Agenda/Symposium Session</para>	
	/// <para>ID: 93dae0b7-f5c9-4ac6-b627-c5c8d5adb8f4</para>	
	/// </summary>
	[SitecoreType(TemplateId="93dae0b7-f5c9-4ac6-b627-c5c8d5adb8f4")] //, Cachable = true
	public partial interface ISymposiumSession : IGlassBase 
	{
								/// <summary>
					/// The Description field.
					/// <para></para>
					/// <para>Field Type: Multi-Line Text</para>		
					/// <para>Field ID: 4b147317-559a-4779-b12d-cbdb151e7a6e</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("4b147317-559a-4779-b12d-cbdb151e7a6e")]
					string Description  {get; set;}
			
								/// <summary>
					/// The End Time field.
					/// <para></para>
					/// <para>Field Type: Datetime</para>		
					/// <para>Field ID: 9459d422-fc77-4aef-bc4a-254e9ed882dd</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("9459d422-fc77-4aef-bc4a-254e9ed882dd")]
					DateTime EndTime  {get; set;}
			
								/// <summary>
					/// The Room field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: e99c9ac9-d3a6-4d38-9463-79c6d3810bec</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("e99c9ac9-d3a6-4d38-9463-79c6d3810bec")]
					string Room  {get; set;}
			
								/// <summary>
					/// The Session Date field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 663de592-7f35-405a-855d-e9357c786a76</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("663de592-7f35-405a-855d-e9357c786a76")]
					string SessionDate  {get; set;}
			
								/// <summary>
					/// The Speakers field.
					/// <para></para>
					/// <para>Field Type: Multi-Line Text</para>		
					/// <para>Field ID: aeb4386d-d32a-4f65-ad6b-1b0b3052a376</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("aeb4386d-d32a-4f65-ad6b-1b0b3052a376")]
					string Speakers  {get; set;}
			
								/// <summary>
					/// The Start Time field.
					/// <para></para>
					/// <para>Field Type: Datetime</para>		
					/// <para>Field ID: 94c0f87e-eaeb-4d28-9e27-09e404f1899f</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("94c0f87e-eaeb-4d28-9e27-09e404f1899f")]
					DateTime StartTime  {get; set;}
			
								/// <summary>
					/// The Title field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 4c99a1fd-700a-4c5e-85c2-9ddc784d1ce2</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("4c99a1fd-700a-4c5e-85c2-9ddc784d1ce2")]
					string Title  {get; set;}
			
								/// <summary>
					/// The Track field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 83b28923-24b5-4aa8-8dfe-eefbc7851844</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("83b28923-24b5-4aa8-8dfe-eefbc7851844")]
					string Track  {get; set;}
			
								/// <summary>
					/// The Type field.
					/// <para></para>
					/// <para>Field Type: Single-Line Text</para>		
					/// <para>Field ID: 800aa83b-f291-4a09-badf-ab07880d5025</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("800aa83b-f291-4a09-badf-ab07880d5025")]
					string Type  {get; set;}
			
								/// <summary>
					/// The Topic Label field.
					/// <para></para>
					/// <para>Field Type: Droplist</para>		
					/// <para>Field ID: 26226e4c-b852-402c-891a-04488fefab29</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("26226e4c-b852-402c-891a-04488fefab29")]
					string TopicLabel  {get; set;}
			
								/// <summary>
					/// The Track Label field.
					/// <para></para>
					/// <para>Field Type: Droplist</para>		
					/// <para>Field ID: 62d8fb1f-a64e-4d03-8d29-cf0d81042685</para>
					/// <para>Custom Data: </para>
					/// </summary>
					[SitecoreField("62d8fb1f-a64e-4d03-8d29-cf0d81042685")]
					string TrackLabel  {get; set;}
			
				}

	
	/// <summary>
	/// SymposiumSession
	/// <para></para>
	/// <para>Path: /sitecore/templates/Sym2018/Feature/Agenda/Symposium Session</para>	
	/// <para>ID: 93dae0b7-f5c9-4ac6-b627-c5c8d5adb8f4</para>	
	/// </summary>
	[SitecoreType(TemplateId="93dae0b7-f5c9-4ac6-b627-c5c8d5adb8f4")] //, Cachable = true
	public partial class SymposiumSession  : GlassBase, ISymposiumSession 
	{
			public const string TemplateIdString = "93dae0b7-f5c9-4ac6-b627-c5c8d5adb8f4";
			public static readonly ID TemplateId = new ID(TemplateIdString);
			public const string TemplateName = "Symposium Session";

					
			public static readonly ID DescriptionFieldId = new ID("4b147317-559a-4779-b12d-cbdb151e7a6e");
			public const string DescriptionFieldName = "Description";
			
					
			public static readonly ID EndTimeFieldId = new ID("9459d422-fc77-4aef-bc4a-254e9ed882dd");
			public const string EndTimeFieldName = "End Time";
			
					
			public static readonly ID RoomFieldId = new ID("e99c9ac9-d3a6-4d38-9463-79c6d3810bec");
			public const string RoomFieldName = "Room";
			
					
			public static readonly ID SessionDateFieldId = new ID("663de592-7f35-405a-855d-e9357c786a76");
			public const string SessionDateFieldName = "Session Date";
			
					
			public static readonly ID SpeakersFieldId = new ID("aeb4386d-d32a-4f65-ad6b-1b0b3052a376");
			public const string SpeakersFieldName = "Speakers";
			
					
			public static readonly ID StartTimeFieldId = new ID("94c0f87e-eaeb-4d28-9e27-09e404f1899f");
			public const string StartTimeFieldName = "Start Time";
			
					
			public static readonly ID TitleFieldId = new ID("4c99a1fd-700a-4c5e-85c2-9ddc784d1ce2");
			public const string TitleFieldName = "Title";
			
					
			public static readonly ID TrackFieldId = new ID("83b28923-24b5-4aa8-8dfe-eefbc7851844");
			public const string TrackFieldName = "Track";
			
					
			public static readonly ID TypeFieldId = new ID("800aa83b-f291-4a09-badf-ab07880d5025");
			public const string TypeFieldName = "Type";
			
					
			public static readonly ID TopicLabelFieldId = new ID("26226e4c-b852-402c-891a-04488fefab29");
			public const string TopicLabelFieldName = "Topic Label";
			
					
			public static readonly ID TrackLabelFieldId = new ID("62d8fb1f-a64e-4d03-8d29-cf0d81042685");
			public const string TrackLabelFieldName = "Track Label";
			
			
	   
						/// <summary>
				/// The Description field.
				/// <para></para>
				/// <para>Field Type: Multi-Line Text</para>		
				/// <para>Field ID: 4b147317-559a-4779-b12d-cbdb151e7a6e</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.DescriptionFieldName)]
				public virtual string Description  {get; set;}
					
						/// <summary>
				/// The End Time field.
				/// <para></para>
				/// <para>Field Type: Datetime</para>		
				/// <para>Field ID: 9459d422-fc77-4aef-bc4a-254e9ed882dd</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.EndTimeFieldName)]
				public virtual DateTime EndTime  {get; set;}
					
						/// <summary>
				/// The Room field.
				/// <para></para>
				/// <para>Field Type: Single-Line Text</para>		
				/// <para>Field ID: e99c9ac9-d3a6-4d38-9463-79c6d3810bec</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.RoomFieldName)]
				public virtual string Room  {get; set;}
					
						/// <summary>
				/// The Session Date field.
				/// <para></para>
				/// <para>Field Type: Single-Line Text</para>		
				/// <para>Field ID: 663de592-7f35-405a-855d-e9357c786a76</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.SessionDateFieldName)]
				public virtual string SessionDate  {get; set;}
					
						/// <summary>
				/// The Speakers field.
				/// <para></para>
				/// <para>Field Type: Multi-Line Text</para>		
				/// <para>Field ID: aeb4386d-d32a-4f65-ad6b-1b0b3052a376</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.SpeakersFieldName)]
				public virtual string Speakers  {get; set;}
					
						/// <summary>
				/// The Start Time field.
				/// <para></para>
				/// <para>Field Type: Datetime</para>		
				/// <para>Field ID: 94c0f87e-eaeb-4d28-9e27-09e404f1899f</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.StartTimeFieldName)]
				public virtual DateTime StartTime  {get; set;}
					
						/// <summary>
				/// The Title field.
				/// <para></para>
				/// <para>Field Type: Single-Line Text</para>		
				/// <para>Field ID: 4c99a1fd-700a-4c5e-85c2-9ddc784d1ce2</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.TitleFieldName)]
				public virtual string Title  {get; set;}
					
						/// <summary>
				/// The Track field.
				/// <para></para>
				/// <para>Field Type: Single-Line Text</para>		
				/// <para>Field ID: 83b28923-24b5-4aa8-8dfe-eefbc7851844</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.TrackFieldName)]
				public virtual string Track  {get; set;}
					
						/// <summary>
				/// The Type field.
				/// <para></para>
				/// <para>Field Type: Single-Line Text</para>		
				/// <para>Field ID: 800aa83b-f291-4a09-badf-ab07880d5025</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.TypeFieldName)]
				public virtual string Type  {get; set;}
					
						/// <summary>
				/// The Topic Label field.
				/// <para></para>
				/// <para>Field Type: Droplist</para>		
				/// <para>Field ID: 26226e4c-b852-402c-891a-04488fefab29</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.TopicLabelFieldName)]
				public virtual string TopicLabel  {get; set;}
					
						/// <summary>
				/// The Track Label field.
				/// <para></para>
				/// <para>Field Type: Droplist</para>		
				/// <para>Field ID: 62d8fb1f-a64e-4d03-8d29-cf0d81042685</para>
				/// <para>Custom Data: </para>
				/// </summary>
				[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Team Development for Sitecore - GlassItem.tt", "1.0")]
				[SitecoreField(SymposiumSession.TrackLabelFieldName)]
				public virtual string TrackLabel  {get; set;}
					
			
	}
}
