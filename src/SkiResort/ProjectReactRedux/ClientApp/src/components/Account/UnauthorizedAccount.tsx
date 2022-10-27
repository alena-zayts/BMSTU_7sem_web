////import * as Styles from '../../styles/components';
////import * as React from 'react';
////import MyButton from '../UI/button/MyButton';
////import { useForm } from 'react-hook-form'
////import { useNavigate } from 'react-router-dom'
////import { useDispatch, useSelector } from 'react-redux';
////import { useEffect } from 'react'
////import { ApplicationState } from '../../store';



////const UnauthorizedAccount = () => {
////    const loading = useSelector((state: ApplicationState) => state.user.loading);
////    const userInfo = useSelector((state: ApplicationState) => state.user.userInfo);
////    const error = useSelector((state: ApplicationState) => state.user.error);
////    const dispatch = useDispatch()
////    const { register, handleSubmit } = useForm()
////    const navigate = useNavigate()

////    // redirect authenticated user to profile screen
////    useEffect(() => {
////        if (userInfo) {
////            navigate('/account')
////        }
////    }, [navigate, userInfo])

////    const submitForm = (data: { userEmail: string, userPassword: string }) => {
////        dispatch(userLogin(data))
////    }

////    return (
////        <form onSubmit={handleSubmit(submitForm)}>
////            <div className='form-group'>
////                <label htmlFor='email'>Email</label>
////                <input
////                    type='email'
////                    className='form-input'
////                    {...register('email')}
////                    required
////                />
////            </div>
////            <div className='form-group'>
////                <label htmlFor='password'>Password</label>
////                <input
////                    type='password'
////                    className='form-input'
////                    {...register('password')}
////                    required
////                />
////            </div>
////            <button type='submit' className='button' disabled={loading}>
////                Login
////            </button>
////        </form>
////    )
////}
////export default UnauthorizedAccount


//////export const UnauthorizedAccount = () => {
//////    return (
//////                //        <div className='unauthorized'>
//////                //    <h1>Unauthorized :(</h1>
//////                //    <span>
//////                //        <NavLink to='/login'>Login</NavLink> to gain access
//////                //    </span>
//////                //</div>

//////        <React.Fragment>
//////            <Styles.HeaderText>Account (unauthorized)</Styles.HeaderText>
//////            <MyButton> LogIn </MyButton>
//////        </React.Fragment>
//////    );
//////}