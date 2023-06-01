import React from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Registration from './Registration';
import Login from "./Login";
import ContentRoutes from "./components/ContentRoutes/ContentRoutes";

const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(

        <Router>
            <Routes>
                <Route path="*" element={<ContentRoutes/>}/>
                <Route path="/login" element={<Login/>}/>
                <Route path="/registration" element={<Registration/>}/>
            </Routes>
        </Router>

);
