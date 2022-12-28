
                using DemoApp.Server.CompiledModels;
                
                public partial class DemoAppDbContext: RuntimeModel
                {
                
                    private static DemoAppDbContext _instance;
                    public static IModel Instance => _instance;
                    
                    static DemoAppDbContext()
                    {
                        var model = new DemoAppDbContext();
                        model.Initialize();
                        //model.Customize();
                        _instance = model;
                    }
        
                    void Initialize()
                    {
                          var autotable = AutoTableEntityType.Create(this);
var childtable = ChildTableEntityType.Create(this);
var grandchild = GrandChildEntityType.Create(this);
var manualchildtable = ManualChildTableEntityType.Create(this);
var manualtable = ManualTableEntityType.Create(this);
var secondchildtable = SecondChildTableEntityType.Create(this);
var sxarcmwithsysgeneratedcolumns = SXARCMWithSysGeneratedColumnsEntityType.Create(this);
var tablewithmsrowversion = TableWithMSRowVersionEntityType.Create(this);
                          ChildTableEntityType.CreateForeignKey1(childtable, autotable);
GrandChildEntityType.CreateForeignKey1(grandchild, childtable);
ManualChildTableEntityType.CreateForeignKey1(manualchildtable, manualtable);
SecondChildTableEntityType.CreateForeignKey1(secondchildtable, autotable);
                          AutoTableEntityType.CreateAnnotations(autotable);
ChildTableEntityType.CreateAnnotations(childtable);
GrandChildEntityType.CreateAnnotations(grandchild);
ManualChildTableEntityType.CreateAnnotations(manualchildtable);
ManualTableEntityType.CreateAnnotations(manualtable);
SecondChildTableEntityType.CreateAnnotations(secondchildtable);
SXARCMWithSysGeneratedColumnsEntityType.CreateAnnotations(sxarcmwithsysgeneratedcolumns);
TableWithMSRowVersionEntityType.CreateAnnotations(tablewithmsrowversion);
                          
                                      AddAnnotation("ProductVersion", "6.0.3");
                                      AddAnnotation("Relational:MaxIdentifierLength",128);
                                      AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);

            
                    } 
                }
            