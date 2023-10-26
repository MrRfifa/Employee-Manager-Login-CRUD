import { BrowserRouter as Router } from "react-router-dom";
// import "./App.css";
import "react-toastify/dist/ReactToastify.css";
import AuthRouter from "./Routes/AuthRouter";
import AdminRouter from "./Routes/AdminRouter";
import AuthVerifyService from "./Services/Auth/AuthVerifyService";

function App() {
  if (AuthVerifyService.AuthVerify() === 0) {
    return (
      <Router>
        <AuthRouter />
      </Router>
    );
  }
  if (AuthVerifyService.AuthVerify() === 1) {
    return (
      <Router>
        <AdminRouter />
      </Router>
    );
  }
}

export default App;
