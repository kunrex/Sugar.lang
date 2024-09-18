using Sugar.Language;

namespace Sugar.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            new UnitTestCompiler("").Initialise();
        }

        [Test]
        public void InputAndOutput()
        {
            string source = "string: name = input(\"whats your name?\"); print(\"hi \" + name + \"!\");";
                
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }
        
        [Test]
        public void VariableDeclaration()
        {
            string source = "int: x;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }
        
        [Test]
        public void VariableInitialisation()
        {   
            string source = "int: x = 10;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void MultiVariableDeclaration()
        {
            string source = "int: x, y = 10 >> 2;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void VariableAssignment()
        {
            string source = "x = 10;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void MultiVariableAssignment()
        {
            string source = "x = y = 10;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void FunctionCalls()
        {
            string source = "Function(10);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GenericFunctionCall()
        {
            string source = "int: x = Function(10)<int>;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ConructorCall()
        {
            string source = "var: x = create Array(10)<int>;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GenericAmbiguity_FunctionCall()
        {
            string source = "int: x = Function(Function2()<x,z> * y);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GenericAmbiguity_Comparison()
        {
            string source = "int: x = Function(Function2()< x, z > y);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void Indexers()
        {
            string source = "int: x = y[y.length - 1] + z[0, 0];";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }
        
        [Test]
        public void Ternary()
        {
            string source = "int: x = 1 + 2 > 3 ? 10 : 4 * 2;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void FunctionDeclarations()
        {
            string source = "Array<T> ReturnArray(int: length)<T; Y : int> { return null; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ForLoop()
        {
            string source = "for(int: i = 0; i < 10; i++) { print(i); }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ForLoopMultiline()
        {
            string source = "for(int: i = 0, k = 1; i < 10 && k < 9; i++, k++) { print(i + k); }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ForLoopSkip()
        {
            string source = "for(;true;) { print(true); }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void WhileLoop()
        {
            string source = "while(true) { print(true); }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void DoWhileLoop()
        {
            string source = "do { print(true); } while(true)";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ForeachLoop()
        {
            string source = "foreach(var: x in array) print(x);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void IfConditions()
        {
            string source = "if(true) print(true); else if(true && false) print(\"somethings wrong\"); else print(false);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void SwitchStatements()
        {
            string source = "switch(x) { case 1: print(1); break; case var: x when x == 10: { print(10); } break; default: break; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ClassDeclarations()
        {
            string source = "class A{   }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void StructDeclarations()
        {
            string source = "struct A{   }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void InterfaceDeclarations()
        {
            string source = "interface A{   }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void EnumDeclarations()
        {
            string source = "enum RockPaperScissors : int {  Rock = 0, Papers = 1, Scissors }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GenericsClassDeclarations()
        {
            string source = "class A<T; Y : int> {   }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void InheritanceClassDeclarations()
        {
            string source = "class A : Class, IInterface {   }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GetSetPropertyDeclarations()
        {
            string source = "int: x { get => 10; set => x = value; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void GetPropertyDeclarations()
        {
            string source = "int: x { get => 10; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void SetPropertyDeclarations()
        {
            string source = "int: x { set => x = value; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ImportStatements()
        {
            string source = "import Namespace.Subnamespace; import class Namespace.Class;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void Describers()
        {
            string source = "[public][abstract] class Abstract {  }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ThrowStatements()
        {
            string source = "throw create Exception(); throw exceptionVariable;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ArrayLiterals()
        {
            string source = "Array<int>: x = create Array(2)<int> { 1, 2 };";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void SubVariableInitialisations()
        {
            string source = "var: x = create Class() { value1 = 10, value2 = \"hi\" };";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ExplicitCastDeclaration()
        {
            string source = "explicit string(int: i) => i + \"\";";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ImplicitCastDeclaration()
        {
            string source = "implicit float(int: i) => i + 0.0;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void OperatorOverloading()
        {
            string source = "operator string +(string: a, string: b) => a.Append(b);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ExtensionMethods()
        {
            string source = "int Invert(int: i) : int => i * -1;";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void IndexerDeclarations()
        {
            string source = "indexer object(int: index) { get => arrayVariable[index]; }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void TryCatchFinally()
        {
            string source = "try { print(10 / 0); } catch(Exception: e) { print(e); } finally { print(\"done\"); }";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void DefaultAndAsType()
        {
            string source = "bool: isClass = default(string) == null; var: intType = astype(int);";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void ActionDeclaration()
        {
            string source = "action: myAction => () { print(\" Actions wooo\"); };";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }

        [Test]
        public void FunctionDeclaration()
        {
            string source = "function<int>: isEqual => (int: a, int: b) { return a == b; };";
            Assert.That(new UnitTestCompiler(source).Compile(), Is.EqualTo(true));
        }
    }
}
