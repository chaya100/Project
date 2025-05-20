import { useState } from 'react';
import './App.css';
import Navbar from './components/Navbar';
import { Provider } from 'react-redux';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import HorizontalLinearStepper from './components/HorizontalLinearStepper.jsx';
import About from './components/about.jsx';
import HomePage from './components/HomePage'; 
import MyProvider from './components/provider.jsx';
import MealsDisplay from './components/MealsDisplay.jsx';

export default function App() {
  return (
    <>    
        <MyProvider>
        <BrowserRouter>
        <Navbar />
        <Routes>
          <Route index element={<HomePage />} />
          <Route path='about' element={<About />} />
          <Route path="calorie-calculator" element={<HorizontalLinearStepper />} />
          <Route path='meals' element={<MealsDisplay />} /> {/* הוספת Route עבור MealsDisplay */}
        </Routes>
        </BrowserRouter>
        </MyProvider>
    </>
  );
}
