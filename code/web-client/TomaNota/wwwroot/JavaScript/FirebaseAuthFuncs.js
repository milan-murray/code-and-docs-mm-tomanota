// This is a test function
function testLog(textIn) {
    console.log("Test " + textIn);
}

async function firebaseCreateUser(email, password) {
    import { getAuth, createUserWithEmailAndPassword } from "firebase/auth";

    const auth = getAuth();
    createUserWithEmailAndPassword(auth, email, password)
        .then((userCredential) => {
            // Signed in 
            const user = userCredential.user;
            console.log("Good");
            // ...
        })
        .catch((error) => {
            const errorCode = error.code;
            const errorMessage = error.message;
            console.log("Bad");
            // ..
        });

   /* try {
        await firebase.auth().createUserWithEmailAndPassword(email, password);
        await firebaseEmailSignIn(email, password);
    } catch (error) {
        var errorResult = error.code + ": " + error.message;
        console.log(errorResult);
        return errorResult;
    };*/
}

async function firebaseGetCurrentUser() {
    var user = await firebase.auth().currentUser;
    if (user) {
        const JsonUser = JSON.stringify(user);
        return JsonUser;
    } else {
        return null;
    }
}
