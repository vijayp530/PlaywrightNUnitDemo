You are working in a Playwright + C# + NUnit automation framework.

Always follow these rules:

- Use BaseTest as the parent class for all tests.
- Reuse ConfigReader for configuration values.
- Use async/await for all Playwright operations.
- Prefer Playwright assertions (`Expect`) for UI validation; use NUnit assertions only where Playwright assertions are not appropriate.
- Do not place assertions inside Page Objects.
- Store test data in JSON under TestData.
- Follow the Page Object Model.
- Prefer locators in this order:
  1. GetByRole
  2. GetByLabel
  3. GetByPlaceholder
  4. GetByTestId
  5. CSS
  6. XPath (last resort)
- Reuse existing utilities instead of creating duplicates.
- Generate maintainable, production-ready code with clear method names ending in `Async`.
