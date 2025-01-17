# Build a simple app with text composables


1. Before you begin
In this codelab, you use Jetpack Compose to build a simple Android app that displays a birthday message on the screen.

Prerequisites
How to create an app in Android Studio.
How to run an app on an emulator or your Android device.
What you'll learn
How to write composable functions, such as Text, Column and Row composable functions.
How to display text in your app in a layout.
How to format the text, such as changing text size.
What you'll build
An Android app that displays a birthday greeting in text format, which looks like this screenshot when done:

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/2ff181d48325023c_856.png)

# 2. Set up a Happy Birthday app

In this task, you set up a project in Android Studio with the Empty Activity template and change the text message to a personalized birthday greeting.

Create an Empty Activity project
In the Welcome to Android Studio dialog, select New Project.
In the New Project dialog, select Empty Activity and then click Next.
In the Name field enter Happy Birthday and then select a minimum API level of 24 (Nougat) in the Minimum SDK field and click Finish.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/b17b607847b0d0ab_856.png)

Wait for Android Studio to create the project files and build the project.
Click fd26b2e3c2870c3.png Run ‘app'.
The app should look like this screenshot:

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/d8299bfc1a82cd57_856.png)


When you created this Happy Birthday app with the Empty Activity template, Android Studio set up resources for a basic Android app, including a Hello Android! message on the screen. In this codelab, you learn how that message gets there, how to change its text to a birthday greeting, and how to add and format additional messages.

What is a user interface (UI)?
The user interface (UI) of an app is what you see on the screen: text, images, buttons, and many other types of elements, and how it's laid out on the screen. It's how the app shows things to the user and how the user interacts with the app.

This image contains a clickable button, text message, and text-input field where users can enter data.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/9a2df39af4122803_856.png)

Clickable button

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/50a9b402fd9037c0_856.png)

Text message inside a Card

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/17794ea52cfb5473_856.png)

Text-input field

Each of these elements is called a UI component. Almost everything you see on the screen of your app is a UI element (also known as a UI component). They can be interactive, like a clickable button or an editable input field, or they can be decorative images.

In the following apps, try to find as many UI components as you can.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/dcb5e11ef39aa76d_856.png)

In this codelab, you work with a UI element that displays text called a Text element.

# 3. What is Jetpack Compose?
Jetpack Compose is a modern toolkit for building Android UIs. Compose simplifies and accelerates UI development on Android with less code, powerful tools, and intuitive Kotlin capabilities. With Compose, you can build your UI by defining a set of functions, called composable functions, that take in data and describe UI elements.

Composable functions
Composable functions are the basic building block of a UI in Compose. A composable function:

Describes some part of your UI.
Doesn't return anything.
Takes some input and generates what's shown on the screen.
Annotations
Annotations are means of attaching extra information to code. This information helps tools like the Jetpack Compose compiler, and other developers understand the app's code.

An annotation is applied by prefixing its name (the annotation) with the @ character at the beginning of the declaration you are annotating. Different code elements, including properties, functions, and classes, can be annotated. Later on in the course, you'll learn about classes.

The following diagram is an example of annotated function:

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/87fe1e19ff89ee9c_856.png)

The following code snippet has examples of annotated properties. You will be using these in the coming codelabs.

```kt
// Example code, do not copy it over

@Json
val imgSrcUrl: String

@Volatile
private var INSTANCE: AppDatabase? = null
```

Annotations with parameters
Annotations can take parameters. Parameters provide extra information to the tools processing them. The following are some examples of the @Preview annotation with and without parameters.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/c7165115f8a9e40b_856.png)

Annotation without parameters

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/e3845e0f058aede9_856.png)

Annotation previewing background

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/28a8df85bf4e80e6_856.png)

Annotation with a preview title

You can pass multiple arguments to the annotation, as shown here.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/895f8d3a229c287a_856.png)

Android studio screenshot showing code and preview

Annotation with a preview title and the system UI (the phone screen)

Jetpack Compose includes a wide range of built-in annotations, you have already seen @Composable and @Preview annotations so far in the course. You will learn more annotations and their usages in the latter part of the course.

Example of a composable function
The Composable function is annotated with the @Composable annotation. All composable functions must have this annotation. This annotation informs the Compose compiler that this function is intended to convert data into UI. As a reminder, a compiler is a special program that takes the code you wrote, looks at it line by line, and translates it into something the computer can understand (machine language).

This code snippet is an example of a simple composable function that is passed data (the name function parameter) and uses it to render a text element on the screen.

```kt
@Composable
fun Greeting(name: String) {
    Text(text = "Hello $name!")
}
```

A few notes about the composable function:

Jetpack Compose is built around composable functions. These functions let you define your app's UI programmatically by describing how it should look, rather than focusing on the process of the UI's construction. To create a composable function, just add the @Composable annotation to the function name.
Composable functions can accept arguments, which let the app logic describe or modify the UI. In this case, your UI element accepts a String so that it can greet the user by name.
Notice the composable functions in code
In Android Studio, open the MainActivity.kt file.
Scroll to the GreetingPreview() function. This composable function helps preview the Greeting() function. As a good practice, functions should always be named or renamed to describe their functionality. Change the name of this function to BirthdayCardPreview().

```kt
@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
        Greeting("Android")
    }
}
```

Composable functions can call other composable functions. In this code snippet, the preview function is calling the Greeting() composable function.

Notice the previous function also has another annotation, a @Preview annotation, with a parameter before the @Composable annotation. You learn more about the arguments passed to the @Preview annotation later in the course.

Composable function names
The compose function that returns nothing and bears the @Composable annotation MUST be named using Pascal case. Pascal case refers to a naming convention in which the first letter of each word in a compound word is capitalized. The difference between Pascal case and camel case is that all words in Pascal case are capitalized. In camel case, the first word can be in either case.

The Compose function:

MUST be a noun: DoneButton()
NOT a verb or verb phrase: DrawTextField()
NOT a nouned preposition: TextFieldWithLink()
NOT an adjective: Bright()
NOT an adverb: Outside()
Nouns MAY be prefixed by descriptive adjectives: RoundIcon()
To learn more see Naming Composable functions.

Example code. Do not copy over

```kt
// Do: This function is a descriptive PascalCased noun as a visual UI element
@Composable
fun FancyButton(text: String) {}


// Do: This function is a descriptive PascalCased noun as a non-visual element
// with presence in the composition
@Composable
fun BackButtonHandler() {}


// Don't: This function is a noun but is not PascalCased!
@Composable
fun fancyButton(text: String) {}


// Don't: This function is PascalCased but is not a noun!
@Composable
fun RenderFancyButton(text: String) {}


// Don't: This function is neither PascalCased nor a noun!
@Composable
fun drawProfileImage(image: ImageAsset) {}
```

# 4. Design pane in Android Studio

Android Studio lets you preview your composable functions within the IDE, instead of installing the app to an Android device or emulator. As you learned in the previous pathway, you can preview what your app looks like in the Design pane in Android Studio.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/2bb27291fa8c8ecc_856.png)

The composable function must provide default values for any parameters to preview it. For this reason, it is recommended not to preview the Greeting() function directly. Instead, you need to add another function, the BirthdayCardPreview() function in this case, that calls the Greeting() function with an appropriate parameter.

```kt
@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
        Greeting("Android")
    }
}
```

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/86a1dd28a40dea11_856.png)

To view your preview:

In the BirthdayCardPreview() function, change the "Android" argument in the Greeting() function to your name.

```kt
@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
        Greeting("James")
    }
}
```

The preview should automatically update.
You should see the updated preview.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/907e7542c84daf9f_856.png)

> Important: The code you added to the BirthdayCardPreview() function with the @Preview annotation is only for previewing in the Design pane in Android Studio. These changes aren't reflected in the app. You learn how to make changes to the app later in this codelab.


# 5. Add a new text element

In this task, you remove the Hello $name! greeting and add a birthday greeting.

Add a new composable function
In the MainActivity.kt file, delete the Greeting() function definition. You will add your own function to display the greeting in the codelab later.
Remove the following code

```kt
@Composable
fun Greeting(name: String, modifier: Modifier = Modifier) {
    Text(
        text = "Hello $name!",
        modifier = modifier
    )
}
```

Inside the onCreate() function, notice that the Greeting() function call is now colored red. This red coloring indicates an error. Hover over the cursor over this function call and Android Studio will display information regarding the error.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/9634619e59248532_856.png)

Delete the Greeting() function call along with its arguments from the onCreate()and BirthdayCardPreview() functions. Your MainActivity.kt file will look similar to this:

```kt
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HappyBirthdayTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                }
            }
        }
    }
}

@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
    }
}
```

Before the BirthdayCardPreview() function, add a new function called GreetingText(). Don't forget to add the @Composable annotation before the function because this will be a compose function describing a Text composable.

```kt
@Composable
fun GreetingText() {
}
```

It's a best practice to have your Composable accept a Modifier parameter, and pass that modifier to its first child. You will learn more about Modifier and child elements in the subsequent tasks and codelabs. For now, add a Modifier parameter to the GreetingText() function.

```kt
@Composable
fun GreetingText(modifier: Modifier = Modifier) {
}
```

Add a message parameter of type String to the GreetingText() composable function.

```kt
@Composable
fun GreetingText(message: String, modifier: Modifier = Modifier) {
}
```

In the GreetingText() function, add a Text composable passing in the text message as a named argument.

```kt
@Composable
fun GreetingText(message: String, modifier: Modifier = Modifier) {
    Text(
        text = message
    )
}
```

This GreetingText() function displays text in the UI. It does so by calling the Text() composable function.

Preview the function
In this task you will preview the GreetingText() function in the Design pane.

Call the GreetingText() function inside the BirthdayCardPreview() function.
Pass a String argument to the GreetingText() function, a birthday greeting to your friend. You can customize it with their name if you like, such as "Happy Birthday Sam!".

```kt
@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
        GreetingText(message = "Happy Birthday Sam!")
    }
}
```

In the Design pane updated automatically. Preview your changes.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/334688e2e89fb19e_856.png)

# 6. Change font size

You added text to your user interface, but it doesn't look like the final app yet. In this task, you learn how to change the size, text color, and other attributes that affect the appearance of the text element. You can also experiment with different font sizes and colors.

Scalable pixels
The scalable pixels (SP) is a unit of measure for the font size. UI elements in Android apps use two different units of measurement: density-independent pixels (DP), which you use later for the layout, and scalable pixels (SP). By default, the SP unit is the same size as the DP unit, but it resizes based on the user's preferred text size under phone settings.

In the MainActivity.kt file, scroll to the Text() composable in the GreetingText() function.
Pass the Text() function a fontSize argument as a second named argument and set it to a value of 100.sp.

```kt
Text(
    text = message,
    fontSize = 100.sp
)
```

Android Studio highlights the .sp code because you need to import some classes or properties to compile your app.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/ba6c753d4eefd1d5_856.png)

Click .sp, which is highlighted by Android Studio.
Click Import in the popup to import the androidx.compose.ui.unit.sp to use the .sp extension property.


> Note: The AndroidX (Android Extension) library contains a set of libraries and classes that help accelerate your app development, by providing you the core functionality. You can access the classes, properties, and other artifacts using the androidx package.

Scroll to the top of the file and notice the import statements, where you should see an import androidx.compose.ui.unit.sp statement, which means that Android Studio adds the package to your file.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/e073e9d3465e080c_856.png)

> Note: If you encounter any problems with importing using Android Studio, you can manually add the imports at the top of the file.

Notice the updated preview of the font size. The reason for overlapping message is you need to specify the line height.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/3bf48548c10f4ea_856.png)

> Note: The sp is an extension property for Int, which creates an sp unit. Similarly, you can use the .sp extension property in other data types, like Float and Double.

Update the Text composable to include the line height.

```kt
@Composable
fun GreetingText(message: String, modifier: Modifier = Modifier) {
    Text(
        text = message,
        fontSize = 100.sp,
        lineHeight = 116.sp,
    )
}
```

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/ef457e9b19d4d495_856.png)

Now you can experiment with different font sizes.

# 7. Add another text element

In your previous tasks, you added a birthday-greeting message to your friend. In this task, you sign your card with your name.

In the MainActivity.kt file, scroll to the GreetingText() function.
Pass the function a from parameter of type String for your signature.

```kt
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier)
```

> Note: The order of the function parameters doesn't matter if you use the named arguments in the function call.

After the birthday message Text composable, add another Text composable that accepts a text argument set to the from value.

```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Text(
        // ...
    )
    Text(
        text = from
    )
}
```

Add a fontSize named argument set to a value of 36.sp.

```kt
Text(
    text = from,
    fontSize = 36.sp
)
```

Scroll to the BirthdayCardPreview() function.
Add another String argument to sign your card, such as "From Emma".

```kt
GreetingText(message = "Happy Birthday Sam!", from = "From Emma")
```

Notice the preview.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/8d148222c669dcad_856.png)

A composable function might describe several UI elements. However, if you don't provide guidance on how to arrange them, Compose might arrange the elements in a way that you don't like. For example, the previous code generates two text elements that overlap each other because there's no guidance on how to arrange the two composables.

In your next task, you will learn how to arrange the composables in a row and in a column.


# 8. Arrange the text elements in a row and column

UI Hierarchy
The UI hierarchy is based on containment, meaning one component can contain one or more components, and the terms parent and child are sometimes used. The context here is that the parent UI elements contain children UI elements, which in turn can contain children UI elements. In this section, you will learn about Column, Row, and Box composables, which can act as parent UI elements.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/9270b7e10f954dcb_856.png)

The three basic, standard layout elements in Compose are Column, Row, and Box composables. You learn more about the Box composable in the next codelab.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/d7df7c362f507d6b_856.png)

Column, Row, and Box are composable functions that take composable content as arguments, so you can place items inside these layout elements. For example, each child element inside a Row composable is placed horizontally next to each other in a row.

```kt
// Don't copy.
Row {
    Text("First Column")
    Text("Second Column")
}
```

These text elements display next to each other on the screen as seen in this image.

The blue borders are only for demonstration purposes and don't display.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/7117f9998760a828_856.png)

Trailing lambda syntax
Notice in the previous code snippet that curly braces are used instead of parentheses in the Row composable function. This is called Trailing Lambda Syntax. You learn about lambdas and trailing lambda syntax in detail later in the course. For now, get familiar with this commonly used Compose syntax.

Kotlin offers a special syntax for passing functions as parameters to functions, when the last parameter is a function.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/6373d65802273065_856.png)

When you pass a function as that parameter, you can use trailing lambda syntax. Instead of putting the function inside the parentheses, you can place it outside the parentheses in curly braces. This is a recommended and common practice in Compose, so you need to be familiar with how the code looks.

For example, the last parameter in the Row() composable function is the content parameter, a function that describes the child UI elements. Suppose you wanted to create a row that contains three text elements. This code would work, but it's very cumbersome to use named parameter for the trailing lambda:

```kt
Row(
    content = {
        Text("Some text")
        Text("Some more text")
        Text("Last text")
    }
)
```

Because the content parameter is the last one in the function signature and you pass its value as a lambda expression (for now, it's okay if you don't know what a lambda is, just familiarize yourself with the syntax), you can remove the content parameter and the parentheses as follows:

```kt
Row {
    Text("Some text")
    Text("Some more text")
    Text("Last text")
}
```

Arrange the text elements in a row
In this task, you arrange the text elements in your app in a row to avoid overlap.

In the MainActivity.kt file, scroll to the GreetingText() function.
Add the Row composable around the text elements so that it shows a row with two text elements. Select the two Text composables, click on the light bulb. Select Surround with widget > Surround with Row.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/7ca98d82742d60b9_856.png)

Now the function should look like this code snippet:

```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Row {
        Text(
            text = message,
            fontSize = 100.sp,
            lineHeight = 116.sp,
        )
        Text(
            text = from,
            fontSize = 36.sp
        )
    }
}
```

Android Studio auto imports Row function for you. Scroll to the top and notice the import section. The import androidx.compose.foundation.layout.Row should have been added.
Observe the updated preview in the Design pane. Temporarily change the font size for the birthday message to 30.sp.


![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/665aa2f1cc85c29_856.png)


The preview looks much better now that there's no overlap. However, this isn't what you want because there's not enough room for your signature. In your next task, you arrange the text elements in a column to resolve this issue.

Arrange the text elements in a column
In this task, it is your turn to change the GreetingText() function to arrange the text elements in a column. The preview should look like this screenshot:

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/d80295e73578e75d_856.png)


Now that you've tried doing this on your own, feel free to check your code against the solution code in this snippet:


```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column {
        Text(
            text = message,
            fontSize = 100.sp,
            lineHeight = 116.sp,
        )
        Text(
            text = from,
            fontSize = 36.sp
        )
    }
}
```

Notice the auto imported package by Android Studio:

```kt
import androidx.compose.foundation.layout.Column
```

Recollect that you need to pass the modifier parameter to the child element in the composables. That means you need to pass the modifier parameter to the Column composable.

```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column(modifier = modifier) {
        Text(
            text = message,
            fontSize = 100.sp,
            lineHeight = 116.sp,
        )
        Text(
            text = from,
            fontSize = 36.sp
        )
    }
}
```


# 9. Add greeting to the app

Once you're happy with the preview, it's time to add the composable to your app on your device or emulator.

In the MainActivity.kt file, scroll to the onCreate() function.
Call the GreetingText() function from the Surface block.
Pass the GreetingText() function, your birthday greeting and signature.
The completed onCreate() function should look like this code snippet:

```kt
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HappyBirthdayTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    GreetingText(message = "Happy Birthday Sam!", from = "From Emma")
                }
            }
        }
    }
}
```

Build and run your app on the emulator.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/59e9c0c6e19748ff_856.png)

Align greeting to the center
To align the greeting in the center of the screen add a parameter called verticalArrangement set it to Arrangement.Center. You will learn more on the verticalArrangement in a later codelab.

![](@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column(
        verticalArrangement = Arrangement.Center,
        modifier = modifier
    ) {
        // ...
    }
})

Add 8.dp padding around the column. It is a good practice to use padding values in increments of 4.dp.

```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column(
        verticalArrangement = Arrangement.Center,
        modifier = modifier.padding(8.dp)
    ) {
        // ...
    }
}
```

To further beautify your app, align the greeting text to the center using textAlign.

```kt
Text(
    text = message,
    fontSize = 100.sp,
    lineHeight = 116.sp,
    textAlign = TextAlign.Center
)
```

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/28c8e62f86323ba4_856.png)

In the above screenshot, only the greeting is center aligned because of the textAlign parameter. The signature, From Emma has the default alignment which is left.

Add padding to the signature and align it to the right.

```kt
Text(
    text = from,
    fontSize = 36.sp,
    modifier = Modifier
        .padding(16.dp)
        .align(alignment = Alignment.End)
)
```

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/82b858f2f79ca9c4_856.png)

Adopt good practice
It is a good practice to pass the modifier attribute(s) along with the modifier from the parent composable. Update the modifier parameter in the GreetingText() as follows:

onCreate()

```kt
Surface(
    //...
) {
    GreetingText(
        message = "Happy Birthday Sam!",
        from = "From Emma",
        modifier = Modifier.padding(8.dp)
    )
}
```

GreetingText()

```kt
@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column(
        verticalArrangement = Arrangement.Center,
        modifier = modifier
    ) {
        // ...
    }
}
```
Build and run your app on the emulator to see the final result.

![](https://developer.android.com/static/codelabs/basic-android-kotlin-compose-text-composables/img/2ff181d48325023c_856.png)

# 10. Get the solution code

The completed MainActivity.kt:

```kt
package com.example.happybirthday

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import com.example.happybirthday.ui.theme.HappyBirthdayTheme

class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HappyBirthdayTheme {
                // A surface container using the 'background' color from the theme
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    GreetingText(
                        message = "Happy Birthday Sam!",
                        from = "From Emma",
                        modifier = Modifier.padding(8.dp)
                    )
                }
            }
        }
    }
}

@Composable
fun GreetingText(message: String, from: String, modifier: Modifier = Modifier) {
    Column(
        verticalArrangement = Arrangement.Center,
        modifier = modifier
    ) {
        Text(
            text = message,
            fontSize = 100.sp,
            lineHeight = 116.sp,
            textAlign = TextAlign.Center
        )
        Text(
            text = from,
            fontSize = 36.sp,
            modifier = Modifier
                .padding(16.dp)
                .align(alignment = Alignment.End)
        )
    }
}

@Preview(showBackground = true)
@Composable
fun BirthdayCardPreview() {
    HappyBirthdayTheme {
        GreetingText(message = "Happy Birthday Sam!", from = "From Emma")
    }
}
```


# 11. Conclusion

You created your Happy Birthday app.

In the next codelab, you add a picture to your app, and change the alignment of the text elements to beautify it.

## Summary
Jetpack Compose is a modern toolkit for building Android UI. Jetpack Compose simplifies and accelerates UI development on Android with less code, powerful tools, and intuitive Kotlin APIs.
The user interface (UI) of an app is what you see on the screen: text, images, buttons, and many other types of elements.
Composable functions are the basic building block of Compose. A composable function is a function that describes some part of your UI.
The Composable function is annotated with the @Composable annotation; this annotation informs the Compose compiler that this function is intended to convert data into UI.
The three basic standard layout elements in Compose are Column, Row, and Box. They are Composable functions that take Composable content, so you can place items inside. For example, each child within a Row will be placed horizontally next to each other.
