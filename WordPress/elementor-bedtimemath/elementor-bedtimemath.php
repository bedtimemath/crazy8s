<?php
/*
 * Elementor Awesomesauce WordPress Plugin
 *
 * @package ElementorBedtimeMath
 *
 * Plugin Name: Elementor Bedtime Math
 * Description: Elementor widgets for the Bedtime Math Website
 * Version: 1.0.0
 * Author: Dug Steen
 * Text Domain: elementor-bedtimemath
 */
define( 'ELEMENTOR_BEDTIMEMATH', __FILE__ );
/**
 * Include the elementor_bedtimemath class.
 */
require plugin_dir_path( ELEMENTOR_BEDTIMEMATH ) . 'class-elementor-bedtimemath-activity-list.php';
require plugin_dir_path( ELEMENTOR_BEDTIMEMATH ) . 'class-elementor-bedtimemath-bmp.php';
require plugin_dir_path( ELEMENTOR_BEDTIMEMATH ) . 'class-elementor-bedtimemath-teaser.php';
