import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Registration from './components/Registration/Registration';
import Login from "./components/Login/Login";
import ContentRoutes from "./components/ContentRoutes/ContentRoutes";
import PrivateRoute from "./components/PrivateRoute/PrivateRoute";

const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(
    <Router>
        <Routes>
            <Route path="*" element={<PrivateRoute/>}/>
            <Route path="/login" element={<Login/>}/>
            <Route path="/registration" element={<Registration/>}/>
        </Routes>
    </Router>
);
