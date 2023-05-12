import React from 'react'
import { Link } from 'react-router-dom'

const Login = props => {
  return (
    <div>Login
        <Link to='/'>Go to home.</Link>
        <Link to='/registration'>Go to registration.</Link>
        
        
    </div>
  )
}

export default Login