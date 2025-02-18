<?php
/**
 * BedtimeMath BMP class.
 *
 * @category   Class
 * @package    ElementorBedtimeMath
 * @subpackage WordPress
 * @author     Dug Steen <dug@bedtimemath.org>
 * @copyright  2023 Bedtime Math
 * @license    https://opensource.org/licenses/GPL-3.0 GPL-3.0-only
 * @link       link(https://www.benmarshall.me/build-custom-elementor-widgets/,
 *             Build Custom Elementor Widgets)
 * @since      1.0.0
 * php version 8.1
 */
namespace ElementorBedtimeMath\Widgets;
use Elementor\Widget_Base;
use Elementor\Controls_Manager;

// Security Note: Blocks direct access to the plugin PHP files.
defined( 'ABSPATH' ) || die();

/**
 * BedtimeMath widget class.
 *
 * @since 1.0.0
 */
class BedtimeMath_BMP extends Widget_Base {
	/**
	 * Class constructor.
	 *
	 * @param array $data Widget data.
	 * @param array $args Widget arguments.
	 */
	public function __construct( $data = array(), $args = null ) {
		parent::__construct( $data, $args );
		wp_register_style( 'bedtimemath-style', plugins_url( '/assets/css/bedtimemath.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
		wp_register_style( 'bedtimemath-bmp-style', plugins_url( '/assets/css/bedtimemath-bmp.css', ELEMENTOR_BEDTIMEMATH ), array(), '1.0.0' );
	}
	/**
	 * Retrieve the widget name.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget name.
	 */
	public function get_name() {
		return 'bedtimemath_bmp';
	}
	/**
	 * Retrieve the widget title.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget title.
	 */
	public function get_title() {
		return __( 'Bedtime Math BMP', 'elementor-bedtimemath' );
	}
	/**
	 * Retrieve the widget icon.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return string Widget icon.
	 */
	public function get_icon() {
		return 'eicon-post-content';
	}
	/**
	 * Retrieve the list of categories the widget belongs to.
	 *
	 * Used to determine where to display the widget in the editor.
	 *
	 * Note that currently Elementor supports only one category.
	 * When multiple categories passed, Elementor uses the first one.
	 *
	 * @since 1.0.0
	 *
	 * @access public
	 *
	 * @return array Widget categories.
	 */
	public function get_categories() {
		return array( 'bedtimemath' );
	}
	
	/**
	 * Enqueue styles.
	 */
	public function get_style_depends() {
		return array( 'bedtimemath-style', 'bedtimemath-bmp-style' );
	}
	/**
	 * Register the widget controls.
	 *
	 * Adds different input fields to allow the user to change and customize the widget settings.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function _register_controls() {
		
		// TITLE / STORY
		$this->start_controls_section(
		  'content_titlestory_section',
		  [
			'label' => __( 'Story', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'story',
			[
				'label' => esc_html__( 'Story', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// WEE ONES
		$this->start_controls_section(
		  'content_weeones_section',
		  [
			'label' => __( 'Wee Ones', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'weeones_question',
			[
				'label' => esc_html__( 'Wee Ones Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'weeones_answer',
			[
				'label' => esc_html__( 'Wee Ones Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// LITTLE KIDS
		$this->start_controls_section(
		  'content_littlekids_section',
		  [
			'label' => __( 'Little Kids', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'littlekids_question',
			[
				'label' => esc_html__( 'Little Kids Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'littlekids_answer',
			[
				'label' => esc_html__( 'Little Kids Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// LITTLE KIDS BONUS
		$this->start_controls_section(
		  'content_littlekids_bonus_section',
		  [
			'label' => __( 'Little Kids Bonus', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'littlekids_bonus_question',
			[
				'label' => esc_html__( 'Little Kids Bonus Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'littlekids_bonus_answer',
			[
				'label' => esc_html__( 'Little Kids Bonus Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// BIG KIDS
		$this->start_controls_section(
		  'content_bigkids_section',
		  [
			'label' => __( 'Big Kids', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'bigkids_question',
			[
				'label' => esc_html__( 'Big Kids Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'bigkids_answer',
			[
				'label' => esc_html__( 'Big Kids Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// BIG KIDS BONUS
		$this->start_controls_section(
		  'content_bigkids_bonus_section',
		  [
			'label' => __( 'Big Kids Bonus', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'bigkids_bonus_question',
			[
				'label' => esc_html__( 'Big Kids Bonus Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'bigkids_bonus_answer',
			[
				'label' => esc_html__( 'Big Kids Bonus Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
		
		// SKYS LIMIT
		$this->start_controls_section(
		  'content_skyslimit_section',
		  [
			'label' => __( 'Sky\'s the Limit', 'elementor-bedtimemath' ),
			'tab' => \Elementor\Controls_Manager::TAB_CONTENT,
		  ]
		);

		$this->add_control(
			'skyslimit_question',
			[
				'label' => esc_html__( 'Sky\'s the Limit Question', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->add_control(
			'skyslimit_answer',
			[
				'label' => esc_html__( 'Sky\'s the Limit Answer', 'elementor-bedtimemath' ),
				'type' => \Elementor\Controls_Manager::WYSIWYG
			]
		);

		$this->end_controls_section();
	}

	/**
	 * Render the widget output on the frontend.
	 *
	 * Written in PHP and used to generate the final HTML.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function render() {
		// read the settings
		$settings = $this->get_settings_for_display();
		$regex = '/\<p\>(.*)\<\/p\>/';

		echo '<div class="bmp">';

		if ( isset($settings['story']) && !empty($settings['story']) ) {
			echo '<div class="bmp-story">' . $settings['story'] . '</div>';
		}

		echo '<div class="bmp-questions">';

		echo '<p>';
		if ( isset($settings['weeones_question']) && !empty($settings['weeones_question']) ) {
			$weeonesQuestion = preg_replace($regex, '$1', $settings['weeones_question']);
			echo '<div class="bmp-weeones-question"><em>Wee Ones:</em> ' . $weeonesQuestion . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['littlekids_question']) && !empty($settings['littlekids_question']) ) {
			$littleKidsQuestion = preg_replace($regex, '$1', $settings['littlekids_question']);
			echo '<div class="bmp-littlekids-question"><em>Little Kids:</em> ' . $littleKidsQuestion . '</div>';
		}
		
		if ( isset($settings['littlekids_bonus_question']) && !empty($settings['littlekids_bonus_question']) ) {
			$littleKidsBonusQuestion = preg_replace($regex, '$1', $settings['littlekids_bonus_question']);
			echo '<div class="bmp-littlekids-bonus-question"><em>Bonus:</em> ' . $littleKidsBonusQuestion . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['bigkids_question']) && !empty($settings['bigkids_question']) ) {
			$bigKidsQuestion = preg_replace($regex, '$1', $settings['bigkids_question']);
			echo '<div class="bmp-bigkids-question"><em>Big Kids:</em> ' . $bigKidsQuestion . '</div>';
		}
		
		if ( isset($settings['bigkids_bonus_question']) && !empty($settings['bigkids_bonus_question']) ) {
			$bigKidsBonusQuestion = preg_replace($regex, '$1', $settings['bigkids_bonus_question']);
			echo '<div class="bmp-bigkids-bonus-question"><em>Bonus:</em> ' . $bigKidsBonusQuestion . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['skyslimit_question']) && !empty($settings['skyslimit_question']) ) {
			$skysLimitQuestion = preg_replace($regex, '$1', $settings['skyslimit_question']);
			echo '<div class="bmp-skyslimit-question"><em>The Sky\'s the Limit:</em> ' . $skysLimitQuestion . '</div>';
		}
		echo '</p>';
		
		echo '</div>';

		echo '<div class="bmp-spacer">Answers:</div>';

		echo '<div class="bmp-answers">';

		echo '<p>';
		if ( isset($settings['weeones_answer']) && !empty($settings['weeones_answer']) ) {
			$weeonesAnswer = preg_replace($regex, '$1', $settings['weeones_answer']);
			echo '<div class="bmp-weeones-answer"><em>Wee Ones:</em> ' . $weeonesAnswer . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['littlekids_answer']) && !empty($settings['littlekids_answer']) ) {
			$littleKidsAnswer = preg_replace($regex, '$1', $settings['littlekids_answer']);
			echo '<div class="bmp-littlekids-answer"><em>Little Kids:</em> ' . $littleKidsAnswer . '</div>';
		}

		if ( isset($settings['littlekids_bonus_answer']) && !empty($settings['littlekids_bonus_answer']) ) {
			$littleKidsBonusAnswer = preg_replace($regex, '$1', $settings['littlekids_bonus_answer']);
			echo '<div class="bmp-littlekids-bonus-answer"><em>Bonus:</em> ' . $littleKidsBonusAnswer . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['bigkids_answer']) && !empty($settings['bigkids_answer']) ) {
			$bigKidsAnswer = preg_replace($regex, '$1', $settings['bigkids_answer']);
			echo '<div class="bmp-bigkids-answer"><em>Big Kids:</em> ' . $bigKidsAnswer . '</div>';
		}

		if ( isset($settings['bigkids_bonus_answer']) && !empty($settings['bigkids_bonus_answer']) ) {
			$bigKidsBonusAnswer = preg_replace($regex, '$1', $settings['bigkids_bonus_answer']);
			echo '<div class="bmp-bigkids-bonus-answer"><em>Bonus:</em> ' . $bigKidsBonusAnswer . '</div>';
		}
		echo '</p>';
		
		echo '<p>';
		if ( isset($settings['skyslimit_answer']) && !empty($settings['skyslimit_answer']) ) {
			$skysLimitAnswer = preg_replace($regex, '$1', $settings['skyslimit_answer']);
			echo '<div class="bmp-skyslimit-answer"><em>The Sky\'s the Limit:</em> ' . $skysLimitAnswer . '</div>';
		}
		echo '</p>';
		
		echo '</div>';

		echo '</div>';
	}
	/**
	 * Render the widget output in the editor.
	 *
	 * Written as a Backbone JavaScript template and used to generate the live preview.
	 *
	 * @since 1.0.0
	 *
	 * @access protected
	 */
	protected function content_template() {
		/* Not needed. See: https://github.com/elementor/elementor/issues/9208 */
	}
}
?>
