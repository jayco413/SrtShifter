# XML Documentation Comment Standards for C#

This document defines the coding style and best practices for XML documentation comments in C#. These comments provide structured metadata for classes, methods, properties, and other members, and are used by IntelliSense, doc generators, and API consumers.

---

## When to Use

Use XML documentation comments (`///`) for:

- All **public** classes, interfaces, methods, properties, fields, and events
- **Protected/internal** members if consumed across assemblies or by tooling
- **Private** members only if necessary for internal tooling or clarity

---

## Core Tags and Usage

### `<summary>`
- Briefly explains **what the member does**
- Use **third person**, **present tense**
- First sentence should be concise (used in IntelliSense)

```csharp
/// <summary>
/// Calculates the area of a rectangle.
/// </summary>
```

---

### `<param>`
- Used once per parameter
- Explain **purpose**, not type

```csharp
/// <param name="width">The width of the rectangle in meters.</param>
/// <param name="height">The height of the rectangle in meters.</param>
```

---

### `<returns>`
- Used only for non-void methods
- Describe the return value clearly

```csharp
/// <returns>The area of the rectangle as a double.</returns>
```

---

### `<exception>`
- List known, expected exceptions thrown by the method

```csharp
/// <exception cref="ArgumentOutOfRangeException">
/// Thrown when width or height is negative.
/// </exception>
```

---

## Optional Tags

- `<remarks>` – Additional detail not suited for `<summary>`
- `<example>` – Code usage examples
- `<see>` – Cross-reference another member (`<see cref="OtherMethod"/>`)
- `<seealso>` – External or related types

---

## Style Guidelines

- Always write **complete sentences**
- Use **proper grammar** and **punctuation**
- Avoid repeating parameter names or types
  - Bad: `<param name="x">An int parameter</param>`
  - Good: `<param name="x">The horizontal offset to apply.</param>`
- Avoid HTML tags unless formatting is needed (`<code>`, `<para>`)
- One blank line between comment block and method declaration is optional but should be consistent

---

## Full Example

```csharp
/// <summary>
/// Retrieves a list of orders for a specific customer.
/// </summary>
/// <param name="customerId">The unique identifier of the customer.</param>
/// <returns>A list of matching orders, or an empty list if none are found.</returns>
/// <exception cref="ArgumentNullException">
/// Thrown if <paramref name="customerId"/> is null or empty.
/// </exception>
public List<Order> GetOrdersByCustomerId(string customerId)
```

---

## Tooling and Enforcement

- Use **StyleCop.Analyzers** for compiler-level rules
- Add `.editorconfig` to enforce consistent spacing and file headers
- Generate external documentation with:
  - [DocFX](https://dotnet.github.io/docfx/)
  - [Sandcastle](https://github.com/EWSoftware/SHFB)
  - Visual Studio `/doc` output

---

## Naming

The formal name for these comments is:

> **XML Documentation Comments**

They are the C# equivalent of Javadoc in Java and use XML tags to describe code structure.

---
```