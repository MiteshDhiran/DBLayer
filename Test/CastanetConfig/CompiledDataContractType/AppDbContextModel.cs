
                using CastApp.Server.CompiledModels;
                
                public partial class CastAppDbContext: RuntimeModel
                {
                
                    private static CastAppDbContext _instance;
                    public static IModel Instance => _instance;
                    
                    static CastAppDbContext()
                    {
                        var model = new CastAppDbContext();
                        model.Initialize();
                        //model.Customize();
                        _instance = model;
                    }
        
                    void Initialize()
                    {
                          var caslist_merged_setting = caslist_merged_settingEntityType.Create(this);
var category_counts_pivot = category_counts_pivotEntityType.Create(this);
var list_enriched = list_enrichedEntityType.Create(this);
                          
                          caslist_merged_settingEntityType.CreateAnnotations(caslist_merged_setting);
category_counts_pivotEntityType.CreateAnnotations(category_counts_pivot);
list_enrichedEntityType.CreateAnnotations(list_enriched);
                          
                                      AddAnnotation("ProductVersion", "6.0.3");
                                      AddAnnotation("Relational:MaxIdentifierLength",128);
                                      AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);

            
                    } 
                }
            