// See https://aka.ms/new-console-template for more information
using TestPrecompiledModel;

Console.WriteLine("Hello, World!");
Test.test_preview_list_load_sp(null, null, false);
Test.DOUnitOfWork_Call_MultipleStoredProcedures();

Test.Test_GetJSONDocument();
Test.Test_SystemGeneratedColumns_being_populated();
Console.WriteLine("Done");
Console.ReadLine();


