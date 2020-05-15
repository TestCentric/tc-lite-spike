
namespace TCLite.Framework.Api
{
	/// <summary>
	/// The ITestListener interface is used internally to receive 
	/// notifications of significant events while a test is being 
    /// run. The events are propagated to clients by means of an
    /// AsyncCallback. NUnit extensions may also monitor these events.
	/// </summary>
	public interface ITestListener
	{
		/// <summary>
		/// Called when a test has just started
		/// </summary>
		/// <param name="test">The test that is starting</param>
		void TestStarted(ITest test);
			
		/// <summary>
		/// Called when a test has finished
		/// </summary>
		/// <param name="result">The result of the test</param>
		void TestFinished(ITestResult result);

		/// <summary>
		/// Called when the test creates text output.
		/// </summary>
		/// <param name="testOutput">A console message</param>
		void TestOutput(TestOutput testOutput);
	}
}
